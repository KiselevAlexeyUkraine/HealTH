using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementRidgitBody : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; 
    [SerializeField] private float sprintSpeed = 8f; 
    [SerializeField] private float jumpForce = 10f; 
    [SerializeField] private float jumpCooldown = 1f; 

    // Звуковые эффекты
    [SerializeField] private AudioClip runSound; 
    [SerializeField] private AudioClip sprintSound; 
    [SerializeField] private AudioClip jumpSound; 
    [SerializeField] private AudioClip damageSound; 

    private Rigidbody rb;
    private InputPlayer _inputPlayer;
    private Animator _animatorPlayer;
    private AudioSource audioSource;
    private ParticleSystem dustParticles; 
    private PlayerStats playerStats; 
    private Camera playerCamera; 

    private float lastJumpTime; 
    private bool isRunningSoundPlaying; 
    private bool isSprintingSoundPlaying; 
    private bool hasJumped; 

    private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private static readonly int SpringHash = Animator.StringToHash("Spring");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int WaveHash = Animator.StringToHash("Wave");
    private static readonly int AttackThePlayerHash = Animator.StringToHash("AttackThePlayer");

    public bool CanJump { get; set; } = true; 
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] Transform _groundChecker;
    [SerializeField] private float _groundCheckerRadius;
    
    public bool isMove = true; 
    public bool isFinishGame; 

    private bool isWave = false;
    private float waveTimer = 0f; 
    [SerializeField] private float waveWaitTime = 10f; 

    [SerializeField] private float stationaryRecoveryAmount = 2f; 
    [SerializeField] private float movingRecoveryAmount = 1f; 
    [SerializeField] private float recoveryInterval = 1f; 
    private float lastRecoveryTime; 

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
        lastJumpTime = -jumpCooldown; 
        lastRecoveryTime = Time.time; 
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

        if (_inputPlayer.Jump && Time.time >= lastJumpTime + jumpCooldown)
        {
            Jump();
        }

        RecoverMotivation();
        
        if (_inputPlayer.Movement.magnitude <= 0.2f && _inputPlayer.Jump)
        {
            if (hasJumped)
            {
                PlaySound(jumpSound, false);
                hasJumped = false;
            }
        }
        else
        {
            hasJumped = false;
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
        if (clip == null || audioSource == null)
        {
            return;
        }
        
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
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