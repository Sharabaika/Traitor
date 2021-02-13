using System.Diagnostics.SymbolStore;
using Characters;
using Items;
using Items.ItemInstances;
using MapObjects;
using UnityEngine;

namespace ScriptableItems
{
    [CreateAssetMenu(fileName = "new item", menuName = "Items/Item")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private new string name = "item name";
        [SerializeField] private Sprite icon;
        [SerializeField,Min(1)] private int maxStack = 1;
        [SerializeField] private string description;
        [SerializeField] private Item item;

        public string Name => name;
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