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

        public void TakeDamage(float damage)
        {
            photonView.RPC("RPC_TakeDamage", RpcTarget.All, damage);
        }

        [PunRPC] private void RPC_TakeDamage(float damage)
        {
            RemainingHealth -= damage;
            Debug.Log(photonView.Owner + " took " + damage + " damage");
        }

        private void Awake()
        {
            RemainingHealth = startingHealth;
        }
    }
}