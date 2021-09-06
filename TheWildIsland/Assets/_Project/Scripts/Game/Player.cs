using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mirror.Examples.Pong
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rb;
        
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
                Movement(-1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Movement(1);
            }
        }

        private void Movement(int direction)
        {
            _rb.AddForce(new Vector2(_speed * direction, 0), ForceMode2D.Impulse);
        }
        
    }
}
