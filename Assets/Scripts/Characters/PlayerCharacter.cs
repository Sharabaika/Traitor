using System;
using Character;
using Photon.Pun;
using UnityEngine;

namespace Characters
{
    public class PlayerCharacter : MonoBehaviourPunCallbacks
    {
        private PlayerMovement _movement;
        private CharacterAnimator _animator;
        private Inventory _inventory;
        private Health _health;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _animator = GetComponent<CharacterAnimator>();
            _inventory = GetComponent<Inventory>();
            _health = GetComponent<Health>();
        }

        [PunRPC] public void SignRoles(int prof, bool isImposter)
        {
            IsImposter = isImposter;
        }

        [PunRPC] public void RelocateTo(Vector3 position)
        {
            _movement.Velocity = Vector3.zero;
            transform.position = position;
        }
        
        public bool IsImposter = false;
    }
}