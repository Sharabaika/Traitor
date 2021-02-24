using Misc;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Health : MonoBehaviourPun, IDamageable
    {
        [SerializeField] private UnityEvent<int> onHealthReduced;

        private int _maxHealth;
        private int _health;

        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
                _health = math.clamp(_health, 0, _maxHealth);
            }
        }

        public int RemainingHealth
        {
            get=> _health;
            set
            {
                value = math.clamp(value, 0, MaxHealth);
                if(value < _health)
                    onHealthReduced?.Invoke(value);
                
                _health = value;
                if(RemainingHealth<=0)
                    GetComponent<PlayerCharacter>().Die();
            }
        }
        public string DamageLog { get; private set; }  = "";

        public void TakeHit(Vector3 hitDirection, Vector3 point, int damage, string damageSource)
        {
            photonView.RPC("RPC_TakeHit", RpcTarget.All, hitDirection, point, damage, damageSource);
        }

        [PunRPC] public void RPC_TakeHit(Vector3 hitDirection, Vector3 point, int damage, string damageSource)
        {
            RemainingHealth -= damage;
            DamageLog += $"took {damage} damage from {damageSource}\n";

            Debug.Log(photonView.Owner + " took " + damage + " damage");
        }
    }
}