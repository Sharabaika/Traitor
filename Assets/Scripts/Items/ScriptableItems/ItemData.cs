using Items.ItemInstances;
using UnityEngine;

namespace Items.ScriptableItems
{
    [CreateAssetMenu(fileName = "new item", menuName = "Items/Item")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField,Min(1)] private int maxStack = 1;
        [SerializeField] private string description;
        [SerializeField] private Item item;
        
        public Sprite Icon => icon;
        public int MaxStack => maxStack;
        public string Description => description;
        public Item Item => item;

        public virtual ItemInstance GetItemInstance()
        {
            return new ItemInstance(this);
        }
    }
}