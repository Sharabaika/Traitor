using System;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableItems
{
    [ExecuteInEditMode]public class ItemContainer : MonoBehaviourPun
    {
        [SerializeField] public UnityEvent onItemsUpdated;
        [SerializeField] public UnityEvent onItemsSynchronized;
        [SerializeField] public UnityEvent OnInventoryReshape;
        [SerializeField] protected ItemSlot[] _itemSlots = new ItemSlot[20];
        
        public int Capacity => _itemSlots.Length;
        private int _previousCapacity;
        
        public ItemSlot GetSlotByIndex(int index)
        {
            return _itemSlots[index];
        }
        
        public void Combine(ItemSlot itemsToAdd, ItemSlot target)
        {
            if(itemsToAdd == target)
                return;
            
            if (itemsToAdd.Item == target.Item)
            {
                var quantity = target.Quantity + itemsToAdd.Quantity;
                quantity = Mathf.Min(quantity, target.Item.MaxStack);
                itemsToAdd.Quantity = itemsToAdd.Quantity - (quantity - target.Quantity);
                target.Quantity = quantity;
            }
            else
            {
                ItemSlot.SwapItems(itemsToAdd, target);
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

        protected void OnValidate()
        {
            if (Capacity != _previousCapacity)
            {
                OnInventoryReshape?.Invoke();
                _previousCapacity = Capacity;
            }
            else
            {
                onItemsUpdated.Invoke();
            }
        }
    }
}