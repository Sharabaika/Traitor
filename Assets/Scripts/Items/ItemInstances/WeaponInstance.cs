using Characters;
using Items.ScriptableItems;
using Misc;
using ScriptableItems;
using UnityEngine;

namespace Items.ItemInstances
{
    public class WeaponInstance : ItemInstance
    {
        public int RemainingAmmo { get; private set; } = 0;
        private WeaponData _data;

        public WeaponInstance(ItemData data,int remainingAmmo): base(data)
        {
            RemainingAmmo = remainingAmmo;
            _data =(WeaponData) data;
        }

        public override void AlternativeUse(PlayerCharacter user)
        {
            // reload
            if(user.Inventory.RemoveOneOf(_data.AmmoTypes) == false)
                return;
        }
        

        public override void UseBy(PlayerCharacter character)
        {
            // Shoot
            var position = character.Inventory.AnchorTransform.position;
            var direction = (character.PointOfLook - position).normalized;
            var ray = new Ray(position, direction);
            
            if (Physics.Raycast(ray,out var hit ,_data.MaxDist, _data.RaycastMasc))
            {
                var damageable = hit.collider.GetComponent<IDamageable>();
                damageable?.TakeHit(direction, hit.point, _data.Damage, _data.DamageSourceDescription);
            }
        }
    }
}