using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mirror.Examples.Pong
{
    public class Player : NetworkBehaviour
    {
        public delegate void PlayerDataHandler(PlayerData playerData);
        public PlayerDataHandler OnPlayerDataUpdate;

        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private GameObject _weaponPos;

        private bool _isGrounded = true;
        private int _direction = 1;

        [SyncVar]
        private int _playerNumber;
        private bool _canMove = false;
        private GameObject _currentWeapon;
        private PlayerData _playerData;
        private bool _isRunning;
        private bool _isJumping;

        public void SetupPlayer(int number)
        {
            _playerNumber = number;
        }

        public void SpawnPosition()
        {
            _canMove = true;

            if (_playerNumber == 1)
            {
                transform.position = new Vector3(15, -10, 0);
            }
            else
            {
                transform.position = new Vector3(-25, -10, 0);
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            NetworkLobby.OnStartGame += SpawnPosition;
            //TODO change weapon
            _currentWeapon = _weaponPos.transform.GetChild(0).gameObject;
        }


        private void FixedUpdate()
        {
            if (isLocalPlayer && _canMove)
            {
                InputListener();
            }

            _playerData = new PlayerData();
        }

        private void CreatePlayerStruct()
        {
            _playerData.OnGround = _isGrounded;
            _playerData.Jump = _isJumping;
            _playerData.Run = _isRunning;
            //_playerData.TakeDamage = _isTakingDamage;

            OnPlayerDataUpdate?.Invoke(_playerData);
        }

        //TODO NEW INPUT SYSTEM
        private void InputListener()
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


            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                Aim();
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
            _currentWeapon.transform.LookAt(Input.mousePosition);
            _currentWeapon.transform.Rotate(0, -90, 0);

        }

        private void FlipPlayer(int newDirection)
        {
            if(_direction != newDirection)
            {
                _direction = newDirection;
                //TODO FIX ROTATION
                if(newDirection == -1)
                {
                    transform.Rotate(0, 180, 0);
                }
                else
                {
                    transform.Rotate(0, -180, 0);
                }
            }

        }

        private void Jump()
        {
            if(_isGrounded)
            {
                _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
                _isJumping = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            LayerMask groundLayer = LayerMask.NameToLayer("Ground");

            if (other.gameObject.layer == groundLayer)
            {
               _isGrounded = true;
               _isJumping = false;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            LayerMask groundLayer = LayerMask.NameToLayer("Ground");

            if (other.gameObject.layer == groundLayer)
            {
              _isGrounded = false;
            }
        }
    }
}
