using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Examples.Pong
{
    public class PlayerAnimationSystem : MonoBehaviour
    {
        [SerializeField] private Player _player;
        private Animator _animator;

        private void Start()
        {
            Initialize();
            SetupDelegates();
        }

        private void OnDestroy()
        {
            RemoveDelegates();
        }

        private void SetupDelegates()
        {
            _player.OnPlayerDataUpdate += ReceiveInputs;
        }

        private void RemoveDelegates()
        {
            _player.OnPlayerDataUpdate -= ReceiveInputs;
        }

        private void Initialize()
        {
            _animator = GetComponent<Animator>();
        }

        private void ReceiveInputs(PlayerData playerData)
        {
            Debug.Log("RECEIVE INPUTS");
            //TODO RECEIVE INPUTS
            Run(playerData.Run);
            SetIsGrounded(playerData.OnGround);
            Jump(playerData.Jump);
        }

        private void Run(bool isRunning)
        {
            _animator.SetBool("isRunning", isRunning);
        }

        private void Jump(bool isJumping)
        {
            _animator.SetBool("isJumping", isJumping);
        }

        private void SetIsGrounded(bool isGrounded)
        {
            _animator.SetBool("isGrounded", isGrounded);
        }

    }
}