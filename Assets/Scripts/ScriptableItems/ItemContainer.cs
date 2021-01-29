using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableItems
{
    [ExecuteInEditMode] class ItemContainer : MonoBehaviour, IItemContainer
    {
        [SerializeField] public UnityEvent onItemsUpdated;
        
        [SerializeField]private ItemSlot[] _itemSlots = new ItemSlot[20];

        public void Combine(ItemSlot itemsToAdd, ItemSlot target)
        {
            if (itemsToAdd.Item == target.Item)
            {
                var quantity = target.Quantity + itemsToAdd.Quantity;
                quantity = Mathf.Min(quantity, target.Item.MaxStack);
                itemsToAdd.Quantity = itemsToAdd.Quantity - (quantity - target.Quantity);
                target.Quantity = quantity;
            }
            else
            {
                ItemSlot.Swap(itemsToAdd, target);
            }
            
            onItemsUpdated.Invoke();
        }

        public bool HasItem(Item item)
        {
            foreach (var itemSlot in _itemSlots)
            {
                if(itemSlot.IsEmpty)
                    continue;
                if (itemSlot.Item == item)
                    return true;
            }

            return false;
        }

        public int Count(Item itemsToCount)
        {
            var quantity = 0;
            foreach (var itemSlot in _itemSlots)
            {
                if(itemSlot.IsEmpty)
                    continue;
                if(itemSlot.Item == itemsToCount)
                    quantity += itemSlot.Quantity;
            }
            return quantity;
        }

        public ItemSlot GetSlotByIndex(int index)
        {
            return _itemSlots[index];
        }
    }
}