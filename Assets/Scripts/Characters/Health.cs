using Misc;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Health : MonoBehaviourPun, IDamageable
    {
        [SerializeField] private float startingHealth;
        [SerializeField] private UnityEvent<float> healthChanged;

        public float RemainingHealth
        {
            get=> _health;
            private set
            {
                _health = value;
                healthChanged?.Invoke(value);
                if(RemainingHealth<=0)
                    GetComponent<PlayerCharacter>().Die();
            }
        }
        private float _health;

        public void TakeHit(float damage)
        {
            photonView.RPC("TakeDamage", RpcTarget.All, damage);
        }

        [PunRPC] private void TakeDamage(float damage)
        {
            RemainingHealth -= damage;
        }

        private void Awake()
        {
            RemainingHealth = startingHealth;
        }
    }
}