using Misc;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Health : MonoBehaviourPun, IDamageable
    {
        [SerializeField] private UnityEvent<int> healthChanged;
        [SerializeField]private int health;

        public string damageLog  = "";

        public int RemainingHealth
        {
            get=> health;
            private set
            {
                health = value;
                healthChanged?.Invoke(value);
                // if(RemainingHealth<=0)
                //     GetComponent<PlayerCharacter>().Die();
            }
        }

        public void TakeHit(Vector3 hitDirection, Vector3 point, int damage, string damageSource)
        {
            photonView.RPC("RPC_TakeHit", RpcTarget.All, hitDirection, point, damage, damageSource);
        }

        [PunRPC] public void RPC_TakeHit(Vector3 hitDirection, Vector3 point, int damage, string damageSource)
        {
            RemainingHealth -= damage;
            damageLog += $"took {damage} damage from {damageSource}\n";

            Debug.Log(photonView.Owner + " took " + damage + " damage");
        }
    }
}