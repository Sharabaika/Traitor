using Characters;
using Items.ItemInstances;
using MapObjects;
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
            itemInstance = instance;
            _weaponInstance = (WeaponInstance) instance;
            _data = (WeaponData) instance.Data;
        }

        public override void Use(PlayerCharacter by, InteractableObject target = null)
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

                photonView.RPC("OnShot_RPC", RpcTarget.All);
            }
            else
            {
                photonView.RPC("OnFailedShot_RPC", RpcTarget.All);
            }
        }

        public override void AlternativeUse(PlayerCharacter by, InteractableObject target = null)
        {
            // Reload
            _weaponInstance.Reload(by);
            photonView.RPC("OnReload_RPC", RpcTarget.All);
        }

        [PunRPC] public void OnShot_RPC()
        {
            onShot?.Invoke();
        }
        
        [PunRPC] public void OnFailedShot_RPC()
        {
            onFailedShot?.Invoke();
        }
        
        [PunRPC] public void OnReload_RPC()
        {
            onReload?.Invoke();
        }
        
        protected override void OnUpdate()
        {
            if (isHidden == false && HasOwner)
            {
                var direction = Owner.PointOfLook - transform.position;
                _targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

                if (OwnerIsLocal)
                {
                    transform.rotation = _targetRotation;
                }
                else
                {
                    // smooth
                    transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation,
                        Time.deltaTime * swingSmoothing);
                }
            }
        }
    }
}