using System;
using Misc;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private float startingHealth;
        [SerializeField] private UnityEvent<float> healthChanged;

        private PhotonView _view;

        public float RemainingHealth
        {
            get=> _health;
            private set
            {
                _health = value;
                healthChanged?.Invoke(value);
            }
        }
        private float _health;

        public void TakeHit(float damage)
        {
            _view.RPC("TakeDamage", RpcTarget.All, damage);
        }

        [PunRPC] private void TakeDamage(float damage)
        {
            RemainingHealth -= damage;
        } 
        
        private void Awake()
        {
            RemainingHealth = startingHealth;

            _view = GetComponent<PhotonView>();
        }
    }
}