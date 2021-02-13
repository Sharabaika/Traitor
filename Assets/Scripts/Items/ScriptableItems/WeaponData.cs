using Items.ItemInstances;
using UnityEngine;

namespace ScriptableItems
{
    [CreateAssetMenu(fileName = "new weapon", menuName = "Items/Weapon")]
    public class WeaponData : ItemData
    {
        [SerializeField] private int damage;
        [SerializeField] private int ammoCapacity;
        [SerializeField] private string damageSourceDescription;
        [SerializeField] private ItemData[] ammoTypes;
        [SerializeField] private float maxDist = 15f;
        [SerializeField] private LayerMask raycastMasc;

        public int Damage => damage;
        public int AmmoCapacity => ammoCapacity;
        public ItemData[] AmmoTypes => ammoTypes;
        public float MaxDist => maxDist;
        public LayerMask RaycastMasc => raycastMasc;
        public string DamageSourceDescription => damageSourceDescription;

        public override ItemInstance GetItemInstance()
        {
            return new WeaponInstance(this, ammoCapacity);
        }
    }
}