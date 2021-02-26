using System.Collections.Generic;
using System.Linq;
using Items;
using Items.ItemInstances;
using Items.ScriptableItems;
using Photon.Pun;
using ScriptableItems;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
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
                if(photonView.IsMine == false)
                    return;
                
                if (ActiveItem != null)
                {
                    ActiveItem.isHidden = true;
                }
                _activeIndex = value;
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
            if(photonView.IsMine == false)
                return;
            
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
                    var itemPrefab = slot.ItemInstance.Data.Item;
                    var path = slot.ItemInstance.Data.PrefabPath;
                    var gameObject = PhotonNetwork.Instantiate(path, anchor.position, Quaternion.identity);
                    
                    gameObject.transform.SetParent(anchor);
                    
                    var item = gameObject.GetComponent<Item>();
                    item.isHidden = true;
                    item.HandlePositioning(_character);
                    item.SetItemInstance(slot.ItemInstance);
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
                PhotonNetwork.Destroy(item.gameObject);
            }
        }

        private void OnItemsUpdated()
        {
            CreateItemRepresentations();
            ActiveIndex = ActiveIndex;
        }
        
        protected override void OnAwake()
        {
            if(photonView.IsMine)
                SetItems(serializedSlots);

            _character = GetComponent<PlayerCharacter>();
            
            _items = new Dictionary<ItemInstance, Item>();
            // CreateItemRepresentations();
            
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

            if (Input.GetKeyDown(KeyCode.Mouse0) && ActiveSlot.IsEmpty == false)
            {
                ActiveItem.Use(_character);
                onItemsUpdated.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.R) && ActiveSlot.IsEmpty == false)
            {
                ActiveItem.AlternativeUse(_character);
                onItemsUpdated.Invoke();
            }
        }
    }
}