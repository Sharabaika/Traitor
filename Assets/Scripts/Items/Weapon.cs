using Misc;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class Weapon : Item
    {
        [SerializeField] private Transform muzzle;
        [SerializeField] private ParticleSystem shootingEffects;

        [SerializeField] private float damage = 1f;
        [SerializeField] private float maxDist = 5f;

        [SerializeField] protected UnityEvent OnShoot;
        [SerializeField] protected GameObject sprite;

        private bool _isHidden = false;

        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                _isHidden = value; 
                sprite.SetActive(!value);
            }
        }

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
            OnShoot?.Invoke();
        }
    }
}