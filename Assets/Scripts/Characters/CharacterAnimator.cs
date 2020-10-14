using System;
using Photon.Pun;
using UnityEngine;

namespace Character
{
    public class CharacterAnimator : MonoBehaviour
    {
        private Animator _animator;
        
        public bool IsLookingRight
        {
            get => _isLookingRight;
            set
            {
                _spriteRenderer.flipX = !value;
                _isLookingRight = value;
            }
        }
        private bool _isLookingRight = true;

        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }
        
        public void Movement(Vector3 velocity)
        {
            _animator.SetBool("isRunning", velocity.sqrMagnitude > 0f);
        }
    }
}