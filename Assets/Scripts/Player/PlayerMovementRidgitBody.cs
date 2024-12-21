using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementRidgitBody : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Скорость движения игрока
    [SerializeField] private float sprintSpeed = 8f; // Скорость спринта
    [SerializeField] private float jumpForce = 10f; // Сила прыжка
    [SerializeField] private float jumpCooldown = 1f; // Время между прыжками

    // Звуковые эффекты
    [SerializeField] private AudioClip runSound; // Звук бега
    [SerializeField] private AudioClip sprintSound; // Звук спринта
    [SerializeField] private AudioClip jumpSound; // Звук прыжка
    [SerializeField] private AudioClip damageSound; // Звук получения урона

    private Rigidbody rb;
    private InputPlayer _inputPlayer;
    private Animator _animatorPlayer;
    private AudioSource audioSource;
    private ParticleSystem dustParticles; // Система частиц пыли
    private PlayerStats playerStats; // Статистика игрока
    private Camera playerCamera; // Ссылка на камеру

    private float lastJumpTime; // Время последнего прыжка
    private bool isRunningSoundPlaying = false; // Флаг для звука бега
    private bool isSprintingSoundPlaying = false; // Флаг для звука спринта
    private bool hasJumped = false; // Флаг для отслеживания, был ли выполнен прыжок

    private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private static readonly int SpringHash = Animator.StringToHash("Spring");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int WaveHash = Animator.StringToHash("Wave");
    private static readonly int AttackThePlayerHash = Animator.StringToHash("AttackThePlayer");

    public bool CanJump { get; set; } = true; // Возможность прыжка
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] Transform _groundChecker;
    [SerializeField] private float _groundCheckerRadius;
    
    public bool isMove = true; // Флаг возможности движения
    public bool isFinishGame; // Флаг завершения игры

    private bool isWave = false;
    private float waveTimer = 0f; // Таймер для отслеживания времени, стояния на месте
    [SerializeField] private float waveWaitTime = 10f; // Время, через которое игрок начинает анимацию wave, если он стоит на месте

    // Переменные для восстановления мотивации
    [SerializeField] private float stationaryRecoveryAmount = 2f; // Количество восстановления мотивации при стоянии
    [SerializeField] private float movingRecoveryAmount = 1f; // Количество восстановления мотивации при движении
    [SerializeField] private float recoveryInterval = 1f; // Интервал времени между восстановлением мотивации
    private float lastRecoveryTime; // Время последнего восстановления мотивации

    private void Start()
    {
        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        _inputPlayer = GetComponent<InputPlayer>();
        _animatorPlayer = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        dustParticles = GetComponentInChildren<ParticleSystem>();
        playerStats = GetComponent<PlayerStats>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        lastJumpTime = -jumpCooldown; // Чтобы игрок мог прыгнуть сразу
        lastRecoveryTime = Time.time; // Инициализация времени последнего восстановления
    }

    private void FixedUpdate()
    {
        HandleWaveAnimation();
        if (!isMove) return;
        HandleMovement();
    }

    private void Update()
    {
        CheckGround();
        
        if (!isMove) return;

        // Проверка на прыжок
        if (_inputPlayer.Jump && Time.time >= lastJumpTime + jumpCooldown)
        {
            Jump();
        }

        // Восстановление мотивации
        RecoverMotivation();

        // Если игрок не прыгает, но стоит на месте, воспроизводим звук прыжка
        if (_inputPlayer.Movement.magnitude <= 0.2f && _inputPlayer.Jump)
        {
            if (hasJumped)
            {
                PlaySound(jumpSound, false);
                hasJumped = false; // Устанавливаем флаг, чтобы предотвратить повторное воспроизведение
                Debug.Log("Прыгаем с места");
            }
        }
        else
        {
            hasJumped = false; // Сбрасываем флаг, если игрок не нажимает кнопку прыжка
        }

        _animatorPlayer.SetFloat(MoveSpeedHash, _inputPlayer.Movement.magnitude);
    }

    private void RecoverMotivation()
    {
        if (Time.time - lastRecoveryTime >= recoveryInterval)
        {
            if (_inputPlayer.Movement.magnitude <= 0.2f) // Игрок стоит на месте
            {
                playerStats.IncreaseMotivation(stationaryRecoveryAmount);
            }
            else // Игрок в движении
            {
                playerStats.IncreaseMotivation(movingRecoveryAmount);
            }
            lastRecoveryTime = Time.time; // Обновляем время последнего восстановления
        }
    }

    private void HandleWaveAnimation()
    {
        if (_inputPlayer.Movement.magnitude <= 0.2f)
        {
            waveTimer += TimeManager.instance.TimeFixedTime; // Увеличиваем таймер, если игрок не двигается
        }
        else
        {
            waveTimer = 0f; // Сбрасываем таймер, если игрок начал движение
        }

        if (waveTimer >= waveWaitTime && !isWave) // Если игрок стоит на месте достаточно долго, начинаем анимацию wave
        {
            isWave = true;
            _animatorPlayer.SetBool(WaveHash, true);
        }
        else if (_inputPlayer.Movement.magnitude > 0f && isWave) // Если игрок начал двигаться, останавливаем анимацию wave
        {
            isWave = false;
            _animatorPlayer.SetBool(WaveHash, false);
        }
    }

    private void HandleMovement()
    {
        Vector3 movement = _inputPlayer.Movement;
        bool isSprinting = _inputPlayer.Spriting && playerStats.Stamina > 0f;

        if (movement.magnitude > 0.2f)
        {
            MovePlayer(movement, isSprinting);
        }
        else if (movement.magnitude < 0.2f && CanJump == true)
        {
            StopMovementEffects();
        }
    }

    private void MovePlayer(Vector3 movement, bool isSprinting)
    {
        if (!isMove) return;

        float speed = isSprinting ? sprintSpeed : moveSpeed;

        if (isSprinting)
        {
            playerStats.DecreaseStamina(Time.fixedDeltaTime * 10f);
        }

        // Получаем направление камеры
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0; // Убираем вертикальную составляющую
        cameraForward.Normalize(); // Нормализуем вектор

        // Получаем направление движения относительно камеры
        Vector3 right = playerCamera.transform.right;
        right.y = 0; // Убираем вертикальную составляющую
        right.Normalize(); // Нормализуем вектор

        Vector3 desiredMoveDirection = (cameraForward * movement.z + right * movement.x).normalized;

        // Поворачиваем персонажа в направлении движения
        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(desiredMoveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.fixedDeltaTime);
        }

        rb.MovePosition(rb.position + desiredMoveDirection * (speed * Time.fixedDeltaTime));

        _animatorPlayer.SetBool(SpringHash, isSprinting);

        if (isSprinting && !dustParticles.isPlaying)
        {
            dustParticles.Play();
        }
        else if (!isSprinting && dustParticles.isPlaying)
        {
            dustParticles.Stop();
        }

        if (isSprinting && !isSprintingSoundPlaying)
        {
            PlaySound(sprintSound, true);
            isSprintingSoundPlaying = true;
            isRunningSoundPlaying = false;
        }
        else if (!isSprinting && !isRunningSoundPlaying)
        {
            PlaySound(runSound, true);
            isRunningSoundPlaying = true;
            isSprintingSoundPlaying = false;
        }
    }

    private void StopMovementEffects()
    {
        _animatorPlayer.SetFloat(MoveSpeedHash, 0f);
        _animatorPlayer.SetBool(SpringHash, false);

        if (dustParticles.isPlaying)
        {
            dustParticles.Stop();
        }

        StopAllSounds();
    }

    private void Jump()
    {
        if (!CanJump) return;
        
        _animatorPlayer.SetBool(WaveHash, false);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        CanJump = false;
        isWave = false;
        lastJumpTime = Time.time;
            
        if (!hasJumped)
        {
            PlaySound(jumpSound, false);
            hasJumped = true;
        }
        waveTimer = 0f;
    }

    private void CheckGround()
    {
        var isGround = Physics.CheckSphere(_groundChecker.position, _groundCheckerRadius, _groundLayer.value);
        
        CanJump = isGround; 
        hasJumped = !isGround; 
        
        _animatorPlayer.SetBool(JumpHash, hasJumped);
    }

    public void FinishGame()
    {
        isMove = false;
        isFinishGame = true;
        StopAllSounds();
        dustParticles.Stop();
        _animatorPlayer.SetBool(SpringHash, false);
        _animatorPlayer.SetFloat(MoveSpeedHash, 0);
    }

    public void TakeDamage()
    {
        PlaySound(damageSound, false);
        _animatorPlayer.SetTrigger(AttackThePlayerHash);
    }

    private void PlaySound(AudioClip clip, bool loop)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }

    private void StopAllSounds()
    {
        audioSource.Stop();
        isRunningSoundPlaying = false;
        isSprintingSoundPlaying = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_groundChecker.position, _groundCheckerRadius);
    }
}