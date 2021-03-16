using System;
using Items.ItemInstances;
using Items.ScriptableItems;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class ItemContainer : MonoBehaviourPun
    {
        
        [SerializeField] public UnityEvent onItemsUpdated;
        [SerializeField] public UnityEvent onItemsSynchronized;
        [SerializeField] public UnityEvent onInventoryReshape;
        [SerializeField] protected ItemContainerSerializableSlot[] serializedSlots = new ItemContainerSerializableSlot[20];
        
        protected ItemSlot[] itemSlots;
        
        public int Capacity => itemSlots?.Length ?? 0;

        private int _previousCapacity;
        
        public ItemSlot GetSlotByIndex(int index)
        {
            return itemSlots[index];
        }

        public virtual void Add(ItemSlot itemsToAdd)
        {
            foreach (var slot in itemSlots)
            {
                if(itemsToAdd.IsEmpty)
                    break;

                if (ItemInstance.CanStack(itemsToAdd.ItemInstance, slot.ItemInstance) || slot.IsEmpty)
                {
                    Combine(itemsToAdd, slot);
                }
            }
            
            onItemsUpdated.Invoke();
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
            
            onInventoryReshape.Invoke();
            onItemsUpdated.Invoke();
        }
        
        public virtual void Resize(int newCapacity)
        {
            var newSlots = new ItemSlot[newCapacity];
            
            for (int i = 0; i < newCapacity; i++)
            {
                if (i < Capacity)
                {
                    newSlots[i] = itemSlots[i];
                }
                else
                {
                    newSlots[i] = new ItemSlot();
                }
            }
            itemSlots = newSlots;
            
            onInventoryReshape.Invoke();
        }

        protected virtual void OnItemsUpdated()
        {
        }

        protected virtual void OnItemsSynchronized()
        {
            
        }

        private void OnEnable()
        {
            onItemsUpdated.AddListener(OnItemsUpdated);
            onItemsSynchronized.AddListener(OnItemsSynchronized);
        }

        private void OnDisable()
        {
            onItemsUpdated.RemoveListener(OnItemsUpdated);
            onItemsSynchronized.RemoveListener(OnItemsSynchronized);
        }

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {

        }

        private void Start()
        {
            if(PhotonNetwork.IsMasterClient)
                SetItems(serializedSlots);
            OnStart();
        }

        protected virtual void OnStart()
        {
            
        }
    }
}