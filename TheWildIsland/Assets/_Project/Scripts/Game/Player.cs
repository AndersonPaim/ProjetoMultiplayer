using UnityEngine;
using CodeMonkey.Utils;
using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace Mirror.Examples.Pong
{
    public class Player : NetworkBehaviour
    {
        public delegate void ShootHandler(Vector3 pos, Quaternion rot, GameObject bullet);
        public static ShootHandler OnShoot;

        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private GameObject _weaponPos;
        [SerializeField] private GameObject _bulletObj;
        [SerializeField] private GameObject _shootPos;
        [SerializeField] private PlayerConnection _connection;
        [SerializeField] private TextMeshPro _nicknameText;
        [SerializeField] private UIManager _uiManager;
        [Header("AUDIO")]
        [SerializeField] private AudioClip _jumpAudio;
        [SerializeField] private GameObject _audioSourceObj;

        public PlayerConnection Connection => _connection;

        private bool _isGrounded = true;
        private int _direction = 1;
        [SyncVar]
        private int _playerNumber;
        private bool _canMove = false;
        private GameObject _currentWeapon;
        private bool _isRunning;
        private bool _isJumping;
        private Animator _anim;

        [SyncVar(hook = nameof(SetNicknameText))]
        public string PlayerName;

        private float _sensitivity = 0.4f;
        private Vector3 _mouseReference;
        private Vector3 _mouseOffset;
        private Vector3 _rotation = Vector3.zero;
        private Vector3 _mousePos;
        private Vector3 _weaponDir;
        private bool _isRotating = false;
        private bool _canWalk = true;
        private bool _canShoot = true;
        private AudioManager _audioManager;

        public void SetupPlayer(int number)
        {
            _playerNumber = number;
        }

        public IEnumerator SpawnDelay()
        {
            yield return new WaitForSeconds(1);
            SpawnPosition();
        }

        public void SpawnPosition()
        {
            _canMove = true;

            if (_playerNumber == 1)
            {
                transform.position = new Vector3(15, -8, 0);
            }
            else
            {
                transform.position = new Vector3(-25, -8, 0);
            }

            StartCoroutine(SetUIPlayers());

        }

        private IEnumerator SetUIPlayers()
        {
            yield return new WaitForSeconds(1);
            _uiManager = FindObjectOfType<UIManager>();
            _uiManager.SetupPlayer(this);
        }

        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            NetworkLobby.OnStartGame += SpawnPosition;
            _currentWeapon = _weaponPos.transform.GetChild(0).gameObject;
            _anim = GetComponent<Animator>();
            _audioManager = Transform.FindObjectOfType<AudioManager>();

            if(!hasAuthority)
            {
                GetComponent<AudioListener>().enabled = false;
            }
        }

        private void Update()
        {

            if (_connection.hasAuthority && isLocalPlayer && _canMove)
            {
                InputListener();
            }

            if(_connection.hasAuthority)
            {
                _anim.SetBool("isRunning", _isRunning);
                _anim.SetBool("isJumping", _isJumping);
                _anim.SetBool("isGrounded", _isGrounded);
            }

            if(_isRotating)
            {
                _mousePos = UtilsClass.GetMouseWorldPosition();
                _weaponDir = (_mousePos - transform.position).normalized;
                float angle = Mathf.Atan2(_weaponDir.y, _weaponDir.x) * Mathf.Rad2Deg;

                if(_direction == 1)
                {
                    if(angle < 90 && angle > -90)
                    {
                        _weaponPos.transform.eulerAngles = new Vector3(0, 0, angle);
                    }
                }
                else
                {
                    if(angle -180 < 90 && angle -180 > -90)
                    {
                        _weaponPos.transform.eulerAngles = new Vector3(0, 180, -angle -180);
                    }
                }

            }
        }

        private void SetNicknameText(string oldValue, string newValue)
        {
            _nicknameText.text = newValue;
        }

        //TODO NEW INPUT SYSTEM
        private void InputListener()
        {
            if(_canWalk)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    Movement(-1);
                    FlipPlayer(-1);
                    _isRunning = true;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    Movement(1);
                    FlipPlayer(1);
                    _isRunning = true;
                }
                else
                {
                    _isRunning = false;
                }
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(_canShoot)
                {
                    Shoot();
                }
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                Aim();
            }
            else
            {
                _isRotating = false;
            }
        }

        private void Movement(int direction)
        {
            Vector3 vel = _rb.velocity;
            vel.x = direction * _speed;

            _rb.velocity = vel;
        }

        private void Aim()
        {
            //Vector3 lookPos = new Vector3(Input.mousePosition.x, 180, Input.mousePosition.z);
            //_currentWeapon.transform.LookAt(lookPos);

            _isRotating = true;
        }

        private void Shoot()
        {
            _canShoot = false;
            StartCoroutine(ShootDelay());
            float vel = _rb.velocity.x;

            if(vel == 0)
            {
                vel = 4.5f;
            }
            else if(vel < -0)
            {
                vel = 9;
            }
            else if(vel > 9)
            {
                vel = 9;
            }

            Debug.Log(vel);

            OnShoot?.Invoke(_shootPos.transform.position, _weaponPos.transform.rotation, _bulletObj);

            _canWalk = false;
            StartCoroutine(WalkDelay());

            float yRot = 0;

            if(_weaponPos.transform.rotation.eulerAngles.z < 180)
            {
                yRot = 0;
            }
            else
            {
                yRot = 360 - _weaponPos.transform.rotation.eulerAngles.z;
            }

            float xRot = 700 - (yRot * 7.77f);

            yRot = yRot * 12;

            if(_direction == 1)
            {
                Vector3 shootKnockback = new Vector3(-xRot * (vel / 4.5f), yRot, 0);
                _rb.AddForce(shootKnockback);
            }
            else
            {
                Vector3 shootKnockback = new Vector3(xRot * (vel / 4.5f), yRot, 0);
                _rb.AddForce(shootKnockback);
            }
        }

        private IEnumerator ShootDelay()
        {
            yield return new WaitForSeconds(0.5f);
            _canShoot = true;
        }

        private void FlipPlayer(int newDirection)
        {
            if(_direction != newDirection)
            {
                Vector3 weaponPos = _weaponPos.transform.eulerAngles;

                _direction = newDirection;
                //TODO FIX ROTATION
                if(newDirection == -1)
                {
                    transform.Rotate(0, 180, 0);
                    _nicknameText.transform.Rotate(0, 180, 0);
                }
                else
                {
                    transform.Rotate(0, -180, 0);
                    _nicknameText.transform.Rotate(0, -180, 0);
                }
            }

        }

        private void Jump()
        {
            if(_isGrounded)
            {
                _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
                _isJumping = true;
                AudioManager.sInstance.PlayAudio(_jumpAudio, gameObject.transform, 1);
            }
        }

        private IEnumerator WalkDelay()
        {
            yield return new WaitForSeconds(0.5f);
            _canWalk = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            LayerMask groundLayer = LayerMask.NameToLayer("Ground");
            LayerMask platformLayer = LayerMask.NameToLayer("Platform");

            if (other.gameObject.layer == groundLayer || other.gameObject.layer == platformLayer)
            {
               _isGrounded = true;
               _isJumping = false;
            }
            else if(other.gameObject.layer == 9)
            {
                if(_connection.hasAuthority)
                {
                    _connection.CmdPlayerDead(this);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            LayerMask groundLayer = LayerMask.NameToLayer("Ground");
            LayerMask platformLayer = LayerMask.NameToLayer("Platform");

            if (other.gameObject.layer == groundLayer || other.gameObject.layer == platformLayer)
            {
                _isGrounded = false;
            }
        }
    }
}
