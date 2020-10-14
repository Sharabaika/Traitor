using Misc;
using Photon.Pun;
using UnityEngine;

namespace Items
{
    public class Weapon : Item
    {
        [SerializeField] private Transform muzzle;
        [SerializeField] private ParticleSystem shootingEffects;

        [SerializeField] private float damage = 1f;
        [SerializeField] private float maxDist = 5f;

        public void Shoot()
        {
            PlayEffects();

            var hit = Physics2D.Raycast(muzzle.position, muzzle.right);

            if (hit)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                damageable?.TakeHit(damage);
            }
        }

        public void PlayEffects()
        {
            shootingEffects.Play();
        }
    }
}