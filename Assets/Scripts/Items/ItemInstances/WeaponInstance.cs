using Characters;
using Misc;
using ScriptableItems;
using UnityEngine;

namespace Items.ItemInstances
{
    public class WeaponInstance : ItemInstance
    {
        public int RemainingAmmo { get; private set; } = 0;

        public WeaponInstance(ItemData data,int remainingAmmo): base(data)
        {
            RemainingAmmo = remainingAmmo;
        }

        public override void UseBy(PlayerCharacter character)
        {
            var data = (WeaponData) Data;

            var position = character.Inventory.AnchorTransform.position;
            var direction = (character.PointOfLook - position).normalized;
            var ray = new Ray(position, direction);
            
            if (Physics.Raycast(ray,out var hit ,data.MaxDist, data.RaycastMasc))
            {
                var damageable = hit.collider.GetComponent<IDamageable>();
                damageable?.TakeHit(direction, hit.point, data.Damage, data.DamageSourceDescription);
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }
}