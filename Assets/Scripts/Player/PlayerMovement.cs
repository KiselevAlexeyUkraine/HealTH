using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f; 
        [SerializeField] private float _sprintSpeed = 8f; 
        [SerializeField] private float _jumpForce = 10f; 
        [SerializeField] private float _jumpCooldown = 1f; 

        [SerializeField] private AudioClip _runSound; 
        [SerializeField] private AudioClip _sprintSound; 
        [SerializeField] private AudioClip _jumpSound; 
        [SerializeField] private AudioClip _damageSound; 

        private Rigidbody _rb;
        private InputPlayer _inputPlayer;
        private Animator _animatorPlayer;
        private AudioSource _audioSource;
        private ParticleSystem _dustParticles; 
        private PlayerStats _playerStats; 
        private Camera _playerCamera; 

        private float _lastJumpTime; 
        private bool _isRunningSoundPlaying; 
        private bool _isSprintingSoundPlaying; 
        private bool _hasJumped; 

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
            _playerCamera = Camera.main;
            _rb = GetComponent<Rigidbody>();
            _inputPlayer = new InputPlayer();
            _animatorPlayer = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _dustParticles = GetComponentInChildren<ParticleSystem>();
            _playerStats = GetComponent<PlayerStats>();
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            _lastJumpTime = -_jumpCooldown; 
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

            if (_inputPlayer.Jump && Time.time >= _lastJumpTime + _jumpCooldown)
            {
                Jump();
            }

            RecoverMotivation();
        
            if (_inputPlayer.Movement.magnitude <= 0.2f && _inputPlayer.Jump)
            {
                if (_hasJumped)
                {
                    PlaySound(_jumpSound, false);
                    _hasJumped = false;
                }
            }
            else
            {
                _hasJumped = false;
            }

            _animatorPlayer.SetFloat(MoveSpeedHash, _inputPlayer.Movement.magnitude);
        }

        private void RecoverMotivation()
        {
            if (Time.time - lastRecoveryTime >= recoveryInterval)
            {
                if (_inputPlayer.Movement.magnitude <= 0.2f)
                {
                    _playerStats.IncreaseMotivation(stationaryRecoveryAmount);
                }
                else
                {
                    _playerStats.IncreaseMotivation(movingRecoveryAmount);
                }
                lastRecoveryTime = Time.time;
            }
        }

        private void HandleWaveAnimation()
        {
            if (_inputPlayer.Movement.magnitude <= 0.2f)
            {
                waveTimer += Time.fixedDeltaTime;
            }
            else
            {
                waveTimer = 0f; 
            }

            if (waveTimer >= waveWaitTime && !isWave) 
            {
                isWave = true;
                _animatorPlayer.SetBool(WaveHash, true);
            }
            else if (_inputPlayer.Movement.magnitude > 0f && isWave) 
            {
                isWave = false;
                _animatorPlayer.SetBool(WaveHash, false);
            }
        }

        private void HandleMovement()
        {
            var movement = _inputPlayer.Movement;
            var isSprinting = _inputPlayer.Spriting && _playerStats.Stamina > 0f;

            if (movement.magnitude > 0.2f)
            {
                MovePlayer(movement, isSprinting);
            }
            else if (movement.magnitude < 0.2f && CanJump)
            {
                StopMovementEffects();
            }
        }

        private void MovePlayer(Vector3 movement, bool isSprinting)
        {
            if (!isMove) return;

            var speed = isSprinting ? _sprintSpeed : _moveSpeed;

            if (isSprinting)
            {
                _playerStats.DecreaseStamina(Time.fixedDeltaTime * 10f);
            }
            
            var cameraForward = _playerCamera.transform.forward;
            cameraForward.y = 0; 
            cameraForward.Normalize(); 
            
            var right = _playerCamera.transform.right;
            right.y = 0; 
            right.Normalize(); 

            var desiredMoveDirection = (cameraForward * movement.z + right * movement.x).normalized;

            if (desiredMoveDirection != Vector3.zero)
            {
                var toRotation = Quaternion.LookRotation(desiredMoveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.fixedDeltaTime);
            }

            _rb.MovePosition(_rb.position + desiredMoveDirection * (speed * Time.fixedDeltaTime));

            _animatorPlayer.SetBool(SpringHash, isSprinting);

            if (isSprinting && !_dustParticles.isPlaying)
            {
                _dustParticles.Play();
            }
            else if (!isSprinting && _dustParticles.isPlaying)
            {
                _dustParticles.Stop();
            }

            if (isSprinting && !_isSprintingSoundPlaying)
            {
                PlaySound(_sprintSound, true);
                _isSprintingSoundPlaying = true;
                _isRunningSoundPlaying = false;
            }
            else if (!isSprinting && !_isRunningSoundPlaying)
            {
                PlaySound(_runSound, true);
                _isRunningSoundPlaying = true;
                _isSprintingSoundPlaying = false;
            }
        }

        private void StopMovementEffects()
        {
            _animatorPlayer.SetFloat(MoveSpeedHash, 0f);
            _animatorPlayer.SetBool(SpringHash, false);

            if (_dustParticles.isPlaying)
            {
                _dustParticles.Stop();
            }

            StopAllSounds();
        }

        private void Jump()
        {
            if (!CanJump) return;
        
            _animatorPlayer.SetBool(WaveHash, false);
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            CanJump = false;
            isWave = false;
            _lastJumpTime = Time.time;
            
            if (!_hasJumped)
            {
                PlaySound(_jumpSound, false);
                _hasJumped = true;
            }
            waveTimer = 0f;
        }

        private void CheckGround()
        {
            var isGround = Physics.CheckSphere(_groundChecker.position, _groundCheckerRadius, _groundLayer.value);
        
            CanJump = isGround; 
            _hasJumped = !isGround; 
        
            _animatorPlayer.SetBool(JumpHash, _hasJumped);
        }

        public void FinishGame()
        {
            isMove = false;
            isFinishGame = true;
            StopAllSounds();
            _dustParticles.Stop();
            _animatorPlayer.SetBool(SpringHash, false);
            _animatorPlayer.SetFloat(MoveSpeedHash, 0);
        }

        public void TakeDamage()
        {
            PlaySound(_damageSound, false);
            _animatorPlayer.SetTrigger(AttackThePlayerHash);
        }

        private void PlaySound(AudioClip clip, bool loop)
        {
            if (clip == null || _audioSource == null)
            {
                return;
            }
        
            _audioSource.clip = clip;
            _audioSource.loop = loop;
            _audioSource.Play();
        }

        private void StopAllSounds()
        {
            _audioSource.Stop();
            _isRunningSoundPlaying = false;
            _isSprintingSoundPlaying = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_groundChecker.position, _groundCheckerRadius);
        }
    }
}