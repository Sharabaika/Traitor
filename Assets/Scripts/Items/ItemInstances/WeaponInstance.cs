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
            var anchor = character.Inventory.AnchorTransform;
            var data = (WeaponData) Data;

            var ray = new Ray(anchor.position, anchor.forward);
            if (Physics.Raycast(ray,out var hit ,data.MaxDist, data.RaycastMasc))
            {
                var damageable = hit.collider.GetComponent<IDamageable>();
                damageable?.TakeDamage(data.Damage);
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }
}