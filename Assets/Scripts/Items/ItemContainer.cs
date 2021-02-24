﻿using System;
using System.Linq;
using Items;
using Items.ItemInstances;
using Items.ScriptableItems;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableItems
{
    public class ItemContainer : MonoBehaviourPun
    {
        [SerializeField] public UnityEvent onItemsUpdated;
        [SerializeField] public UnityEvent onItemsSynchronized;
        [SerializeField] public UnityEvent onInventoryReshape;
        [SerializeField] protected ItemContainerSerializableSlot[] serializedSlots = new ItemContainerSerializableSlot[20];
        
        protected ItemSlot[] itemSlots;
        
        public int Capacity => itemSlots.Length;
        private int _previousCapacity;
        
        public ItemSlot GetSlotByIndex(int index)
        {
            return itemSlots[index];
        }
        
        public void Combine(ItemSlot itemsToAdd, ItemSlot target)
        {
            if(itemsToAdd == target)
                return;
            
            if (ItemInstance.CanStack(itemsToAdd.ItemInstance, target.ItemInstance))
            {
                var quantity = target.Quantity + itemsToAdd.Quantity;
                quantity = Mathf.Min(quantity, target.ItemInstance.Data.MaxStack);
                itemsToAdd.Quantity = itemsToAdd.Quantity - (quantity - target.Quantity);
                target.Quantity = quantity;
            }
            else
            {
                ItemSlot.SwapItems(itemsToAdd, target);
            }
            
            onItemsUpdated.Invoke();
            
        }

        public bool HasItem(ItemData itemData)
        {
            foreach (var itemSlot in itemSlots)
            {
                if(itemSlot.IsEmpty)
                    continue;
                if (itemSlot.ItemInstance.Data == itemData)
                    return true;
            }

            return false;
        }

        public bool RemoveOne(ItemData itemToRemove)
        {
            foreach (var slot in itemSlots)
            {
                if (slot.ItemInstance.Data == itemToRemove)
                {
                    slot.Quantity--;
                    onItemsUpdated?.Invoke();
                    
                    return true;
                }
            }

            return false;
        }
        
        public bool RemoveOneOf(ItemData[] itemsToRemove)
        {
            foreach (var itemToRemove in itemsToRemove)
            {
                if (RemoveOne(itemToRemove))
                    return true;
            }

            return false;
        }

        public int Count(ItemData itemsDataToCount)
        {
            var quantity = 0;
            foreach (var itemSlot in itemSlots)
            {
                if(itemSlot.IsEmpty)
                    continue;
                if(itemSlot.ItemInstance.Data == itemsDataToCount)
                    quantity += itemSlot.Quantity;
            }
            return quantity;
        }

        public void SetItems(ItemContainerSerializableSlot[] items)
        {
            itemSlots = new ItemSlot[items.Length];
            for (int i = 0; i < Capacity; i++)
            {
                var slot = items[i];
                if (slot.IsEmpty)
                {
                    itemSlots[i] = new ItemSlot();
                }
                else
                {
                    itemSlots[i] = new ItemSlot(slot.quantity, slot.data.GetItemInstance());
                }
            }
            
            if(photonView.IsMine)
            {
                onInventoryReshape.Invoke();
                // onItemsUpdated.Invoke();
            }
        }

        private void Awake()
        {
            SetItems(serializedSlots);
            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }

        protected void OnValidate()
        {
            // if (Capacity != _previousCapacity)
            // {
            //     OnInventoryReshape?.Invoke();
            //     _previousCapacity = Capacity;
            // }
            // else
            // {
            //     onItemsUpdated.Invoke();
            // }
        }
    }
}