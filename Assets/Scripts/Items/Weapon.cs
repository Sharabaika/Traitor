using Characters;
using Misc;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class Weapon : Item
    {
        [SerializeField] private Transform muzzle;
        [SerializeField] private float damage = 1f;
        [SerializeField] private float maxDist = 5f;


        public override void Use(PlayerCharacter by)
        {
            base.Use(by);
            
            OnUse?.Invoke();

            var hit = Physics2D.Raycast(muzzle.position, muzzle.right);

            if (hit)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                damageable?.TakeDamage(damage);
            }
        }
    }
}