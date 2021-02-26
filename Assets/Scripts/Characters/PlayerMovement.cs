using Character;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Characters
{
    [RequireComponent(typeof(PhotonView), typeof(CharacterAnimator))]
    public class PlayerMovement : MonoBehaviourPun
    {
        [SerializeField] private float speed = 1.5f;
        [SerializeField] private float minSpeed = 0.01f;
        
        private CharacterAnimator _animator;
        private PlayerCharacter _character;
        private Rigidbody _rigidbody;

        private bool _isFrozen = false;

        public bool IsFrozen
        {
            get => _isFrozen;
            set
            {
                _isFrozen = value;
                if(value)
                    _rigidbody.velocity = Vector3.zero;
            }
        }

        private void Start()
        {
            _animator = GetComponent<CharacterAnimator>();
            _character = GetComponent<PlayerCharacter>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (photonView.IsMine)
            {
                Vector3 velocity;
                if (IsFrozen)
                {
                    velocity = Vector3.zero;
                }
                else
                {
                    var dir = new Vector3(
                        Input.GetAxisRaw("Horizontal"),
                        0f,
                        Input.GetAxisRaw("Vertical")).normalized;
                    velocity = dir * speed;
                }

                _rigidbody.velocity = velocity;

                _animator.Movement(_rigidbody.velocity);
                _animator.IsLookingRight = (_character.PointOfLook.x - transform.position.x) > 0;
            }
        }
    }
}