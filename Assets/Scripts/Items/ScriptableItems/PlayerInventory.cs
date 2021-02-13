using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Items;
using Items.ItemInstances;
using Items.ScriptableItems;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace ScriptableItems
{
    public class PlayerInventory : ItemContainer
    {
        [SerializeField] public UnityEvent<ItemSlot> OnActiveItemChanged;
        [SerializeField] public UnityEvent<int> OnActiveSlotChanged;

        [SerializeField] private Transform anchor;

        private Dictionary<ItemInstance, Item> _items;
        private PlayerCharacter _character;
        private int _activeIndex;
        
        public ItemSlot ActiveSlot => itemSlots[_activeIndex];
        public Item ActiveItem { get; private set; }
        public Transform AnchorTransform => anchor;

        public int ActiveIndex
        {
            get => _activeIndex;
            set
            {
                if (ActiveItem != null)
                {
                    ActiveItem.isHidden = true;
                }
                _activeIndex = value;
                // if (ActiveSlot.ItemInstance is null)
                if (ActiveSlot.IsEmpty)
                {
                    ActiveItem = null;
                }
                else
                {
                    if (_items.TryGetValue(ActiveSlot.ItemInstance, out var item))
                    {
                        ActiveItem = item;
                        ActiveItem.isHidden = false;
                    }
                    else
                    {
                        ActiveItem = null;
                    }
                }
                
                
                OnActiveSlotChanged.Invoke(value);
                OnActiveItemChanged.Invoke(ActiveSlot);
            }
        }

        private void CreateItemRepresentations()
        {
            // check inventory slots
            for (int i = 0; i < Capacity; i++)
            {
                var slot = itemSlots[i];
                if(slot.IsEmpty)
                    continue;

                if (_items.TryGetValue(slot.ItemInstance, out var representation))
                {
                }
                else
                {
                    // create missing item
                    var item = Instantiate(slot.ItemInstance.Data.Item, anchor, false);
                    item.isHidden = true;
                    item.HandlePositioning(_character);
                    _items.Add(slot.ItemInstance, item);
                }
            }
            
            // remove redundant items
            var itemsToRemove = new List<ItemInstance>();
            var itemsToDestroy = new List<Item>();
            foreach (var pair in _items)
            {
                var itemInstance = pair.Key;
                if (itemSlots.Any((slot => slot.ItemInstance == itemInstance)) == false)
                {
                    itemsToRemove.Add(itemInstance);
                    itemsToDestroy.Add(pair.Value);
                }
            }

            foreach (var itemInstance in itemsToRemove)
            {
                _items.Remove(itemInstance);
            }

            foreach (var item in itemsToDestroy)
            {
                Destroy(item.gameObject);
            }
        }

        private void OnItemsUpdated()
        {
            CreateItemRepresentations();
            ActiveIndex = ActiveIndex;
        }
        
        [PunRPC] public void UseActiveItem()
        {
            if(ActiveSlot.IsEmpty)
                return;
            if (photonView.IsMine)
                ActiveSlot.ItemInstance.UseBy(_character);
            ActiveItem.Use(_character);
        }

        protected override void OnAwake()
        {
            _character = GetComponent<PlayerCharacter>();
            
            _items = new Dictionary<ItemInstance, Item>();
            CreateItemRepresentations();
            
            ActiveIndex = 0;
        }

        private void OnEnable()
        {
            onItemsUpdated.AddListener(OnItemsUpdated);
            onItemsSynchronized.AddListener(OnItemsUpdated);
        }

        private void OnDisable()
        {
            onItemsUpdated.RemoveListener(OnItemsUpdated);
            onItemsSynchronized.RemoveListener(OnItemsUpdated);
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;

            // active slot
            for (int i = 0; i < Capacity; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    ActiveIndex = i;
                    break;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                photonView.RPC("UseActiveItem", RpcTarget.All);
            }
        }
    }
}