using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mirror.Examples.Pong
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _sprite;
        
        private bool _isGrounded = true;

        [SyncVar (hook = nameof(FlipPlayer))]
        private int _direction;

        [SyncVar]
        private int _playerNumber;
        private bool _canMove = false;

        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            NetworkLobby.OnStartGame += SpawnPosition;
        }

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

        private void FixedUpdate()
        {
            if (isLocalPlayer && _canMove)
            {
                InputListener();
            }
        }

        //TODO NEW INPUT SYSTEM
        private void InputListener()
        {
            if (Input.GetKey(KeyCode.A))
            {
                _direction = -1;
                CmdChangeDirection(-1);
                Movement(-1);

                if(isClient)
                {
                    FlipPlayer(0, _direction);
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                _direction = 1;
                CmdChangeDirection(1);
                Movement(1);

                if(isClient)
                {
                    FlipPlayer(0, _direction);
                }
            }
            if(Input.GetKey(KeyCode.Space))
            {
                Jump();

            }
        }

        private void Movement(int direction)
        {
            Vector3 vel = _rb.velocity;
            vel.x = direction * _speed;

            _rb.velocity = vel;
        }

        [Command]
        private void CmdChangeDirection(int direction)
        {
            _direction = direction;
            FlipPlayer(0, direction);
        }

        private void FlipPlayer(int oldDirection, int newDirection)
        {
            if(newDirection == -1)
            {
                _sprite.flipX = true;
            }
            else
            {
                _sprite.flipX = false;
            }
        }

        private void Jump()
        {
            if(_isGrounded)
            {
                _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            LayerMask groundLayer = LayerMask.NameToLayer("Ground");

            if (other.gameObject.layer == groundLayer)
            {
               _isGrounded = true;
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
