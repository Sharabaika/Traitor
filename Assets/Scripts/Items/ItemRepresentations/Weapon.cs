using Characters;
using Items.ItemInstances;
using Misc;
using Photon.Pun;
using ScriptableItems;
using UnityEngine;
using UnityEngine.Events;

namespace Items.ItemRepresentations
{
    public class Weapon : Item
    {
        [SerializeField] private Transform muzzle;
        
        [SerializeField] private UnityEvent onShot;
        [SerializeField] private UnityEvent onFailedShot;
        [SerializeField] private UnityEvent onReload;
        
        [SerializeField] private float swingSmoothing = 15;

        private WeaponInstance _weaponInstance;
        private WeaponData _data;
        private Quaternion _targetRotation;
        
        
        public override void SetItemInstance(ItemInstance instance)
        {
            // TODO unsafe
            _weaponInstance = (WeaponInstance) instance;
            _data = (WeaponData) instance.Data;
        }

        public override void Use(PlayerCharacter by)
        {
            // Shoot
            if (_weaponInstance.WasteAmmo())
            {
                var position = muzzle.position;
                var direction = muzzle.forward;
                var ray = new Ray(position, direction);

                if (Physics.Raycast(ray, out var hit, _data.MaxDist, _data.RaycastMasc))
                {
                    var damageable = hit.collider.GetComponent<IDamageable>();
                    damageable?.TakeHit(direction, hit.point, _data.Damage, _data.DamageSourceDescription);
                }

                photonView.RPC("OnShot", RpcTarget.All);
            }
            else
            {
                photonView.RPC("OnFailedShot", RpcTarget.All);
            }
        }

        public override void AlternativeUse(PlayerCharacter by)
        {
            // Reload
            _weaponInstance.Reload(by);
            photonView.RPC("OnReload", RpcTarget.All);
        }

        [PunRPC] public void OnShot()
        {
            onShot?.Invoke();
        }
        
        [PunRPC] public void OnFailedShot()
        {
            onFailedShot?.Invoke();
        }
        [PunRPC] public void OnReload()
        {
            onReload?.Invoke();
        }
        protected override void OnUpdate()
        {
            if (isHidden == false && HasOwner)
            {
                // position + direction = point
                // direction = point - position
                
                var direction = Owner.PointOfLook - transform.position;
                
                _targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

                if (OwnerIsLocal)
                {
                    transform.rotation = _targetRotation;
                }
                else
                {
                    // rough
                    // transform.rotation = 
                    // Quaternion.RotateTowards(transform.rotation, _targetRotation, Time.deltaTime * swingSpeed);

                    // smooth
                    transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation,
                        Time.deltaTime * swingSmoothing);
                }
            }
        }
    }
}