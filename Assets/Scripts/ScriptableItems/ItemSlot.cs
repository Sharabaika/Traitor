using System;
using UnityEngine;

namespace ScriptableItems
{
    [Serializable]public class ItemSlot
    {
        [SerializeField]private Item item;
        [SerializeField,Min(0)] private int quantity = 0;

        public Item Item
        {
            get => item;
            set
            {
                item = value;
                if (value is null)
                    quantity = 0;
                else quantity = 1;
            }
        }

        public int Quantity
        {
            get => quantity;
            set
            {
                if(Item is null)
                    return;
                quantity = Mathf.Min(value, Item.MaxStack);
                if (Quantity == 0)
                    item = null;
            }
        }

        public bool IsEmpty => Quantity == 0 || Item is null;

        public static void Swap(ItemSlot a, ItemSlot b)
        {
            var tItem = a.item;
            var tQuantity = a.quantity;
            a.Item = b.Item;
            a.Quantity = b.Quantity;
            b.Item = tItem;
            b.Quantity = tQuantity;
        }
    }
}