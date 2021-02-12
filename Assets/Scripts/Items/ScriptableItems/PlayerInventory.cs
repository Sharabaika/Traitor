using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Items;
using Items.ItemInstances;
using Items.ScriptableItems;
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
                if (ActiveSlot.ItemInstance is null)
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

        public Transform AnchorTransform => anchor;

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
                    _items.Add(slot.ItemInstance, item);
                }
            }
            // remove redundant items
            foreach (var itemInstance in _items.Keys)
            {
                if (itemSlots.Any((slot => slot.ItemInstance == itemInstance)) == false)
                {
                    var item = _items[itemInstance];
                    Destroy(item.gameObject);
                    _items.Remove(itemInstance);
                }
            }
        }

        private void OnItemsUpdated()
        {
            CreateItemRepresentations();
            ActiveIndex = ActiveIndex;
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
        }

        private void OnDisable()
        {
            onItemsUpdated.RemoveListener(OnItemsUpdated);
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
            
            // look
            if(ActiveSlot.IsEmpty == false)
                ActiveItem.HandlePositioning(_character);
        }
    }
}