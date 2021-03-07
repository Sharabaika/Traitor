using System;
using System.Text;
using ExitGames.Client.Photon;
using Items;
using Items.ScriptableItems;
using Photon.Pun;
using ScriptableItems;
using UnityEngine;

namespace Networking.Items
{
    public class ItemContainerView : MonoBehaviourPunCallbacks
    {
        [SerializeField] private ItemList itemList;
        private ItemContainer _container;

        private string GetKey()=> "ItemContainer"+photonView.ViewID.ToString();

        private void SendItems()
        {
            var builder = new StringBuilder();

            var key = GetKey();
            
            for (var i = 0; i < _container.Capacity; i++)
            {
                var slot = _container.GetSlotByIndex(i);
                builder.Append(SerializeSlot(slot)).Append(' ');
            }
            var hash = new Hashtable {{key, builder.ToString()}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

        }

        private void SetItems(Hashtable properties)
        {
            if(photonView is null)
                return;

            if (properties.TryGetValue(GetKey(), out var data))
            {

                var str = (string) data;
                var slotsData = str.TrimEnd().Split(' ');
                
                var needToResize = slotsData.Length != _container.Capacity;
                if (needToResize)
                    _container.Resize(slotsData.Length);
                
                var i = 0;
                foreach (var slotData in slotsData)
                {
                    var containerSlot = _container.GetSlotByIndex(i);
                    i++;
                    
                    if(SerializeSlot(containerSlot) == slotData)
                        continue;

                    var parsedSlot = slotData.Split(':');

                    var q =int.Parse(parsedSlot[0]);
                    containerSlot.Quantity = q;
                    if (q > 0)
                    {
                        var id = int.Parse(parsedSlot[1]);
                        var state = parsedSlot[2];
                        containerSlot.ItemInstance =itemList.GetItemInstance(id);
                        containerSlot.ItemInstance.DeserializeState(state);
                    }
                }
                _container.onItemsSynchronized?.Invoke();
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if(false)
                return;
            SetItems(propertiesThatChanged);
        }

        private string SerializeSlot(ItemSlot slot)
        {
            if (slot.IsEmpty)
                return "0";
            
            var builder = new StringBuilder();

            var quantity = slot.Quantity;
            var id = itemList.GetItemDataID(slot.ItemInstance);
            var state = slot.ItemInstance.SerializeState();

            builder.
                    Append(quantity).Append(":").
                    Append(id).Append(":").
                    Append(state);
            
            return builder.ToString();
        }

        private void Awake()
        {
            var props = PhotonNetwork.CurrentRoom.CustomProperties;
            _container = GetComponent<ItemContainer>();
            SetItems(props);
        }

        public override void OnEnable()
        {
            _container.onItemsUpdated.AddListener(SendItems);
            _container.onInventoryReshape.AddListener(SendItems);
            base.OnEnable();
        }

        public override void OnDisable()
        {
            _container.onItemsUpdated.RemoveListener(SendItems);
            _container.onInventoryReshape.RemoveListener(SendItems);
            base.OnDisable();
        }
    }
}