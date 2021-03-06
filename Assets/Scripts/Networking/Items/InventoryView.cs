using System;
using Characters;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using ScriptableItems;
using UnityEngine;

namespace Networking.Items
{
    public class InventoryView : MonoBehaviourPunCallbacks
    {
        [SerializeField] private ItemList itemList;
        private PlayerInventory _inventory;

        // unnecessary
        public void SendActiveItemSlot(int index)
        {
            if(!photonView.IsMine)
                return;

            var hash = new Hashtable {{"ActiveSlot", index}};
            photonView.Owner.SetCustomProperties(hash);
        }
        public void SendItems()
        {
            if( ! (photonView.IsMine || PhotonNetwork.IsMasterClient) )
                return;

            var hash = new Hashtable{{"Capacity",_inventory.Capacity}};
            
            for (var i = 0; i < _inventory.Capacity; i++)
            {
                var slot = _inventory.GetSlotByIndex(i);

                var quantity = slot.Quantity;
                hash.Add("SlotQuantity" + i.ToString(), quantity);

                if (slot.IsEmpty == false)
                {
                    var id = itemList.GetItemDataID(slot.ItemInstance);
                    hash.Add("SlotItem" + i.ToString(), id);
                    var state = slot.ItemInstance.SerializeState();
                    hash.Add("SlotState" + i.ToString(), state);
                }
            }

            photonView.Owner.SetCustomProperties(hash);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if(targetPlayer != photonView.Owner)
                return;
            
            // inventory slots
            var needToUpdateInventory = false;
            for (var i = 0; i < _inventory.Capacity; i++)
            {
                if(changedProps.TryGetValue("SlotQuantity"+i.ToString(),  out var quantity))
                {
                    var slot = _inventory.GetSlotByIndex(i);
                    var q = (int) quantity;
                    
                    needToUpdateInventory = q!=slot.Quantity;
                    slot.Quantity = q;
                }

                if(changedProps.TryGetValue("SlotItem"+i.ToString(),  out var ID))
                {
                    var slot = _inventory.GetSlotByIndex(i);
                    slot.ItemInstance = itemList.GetItem((int)ID).GetItemInstance();
                    needToUpdateInventory = true;
                }

                if(changedProps.TryGetValue("SlotState"+i.ToString(),  out var state))
                {
                    var slot = _inventory.GetSlotByIndex(i);
                    slot.ItemInstance.DeserializeState((string) state);
                    needToUpdateInventory = true;
                }
            }
            if(needToUpdateInventory)
                _inventory.onItemsSynchronized?.Invoke();
            
            // Active slot
            // if (changedProps.TryGetValue("ActiveSlot", out var index))
            // {
            //     inventory.ActiveIndex = (int)index;
            // }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            _inventory.onItemsUpdated.AddListener(SendItems);
            _inventory.onInventoryReshape.AddListener(SendItems);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _inventory.onItemsUpdated.RemoveListener(SendItems);
            _inventory.onInventoryReshape.RemoveListener(SendItems);
        }

        private void Awake()
        {
            _inventory = GetComponent<PlayerInventory>();
            
            // if (photonView.Owner != PhotonNetwork.LocalPlayer)
            // {
            //     var index = photonView.Owner.CustomProperties["ActiveSlot"];
            //     if(index is null)
            //     {
            //         return;
            //     }
            //
            //     inventory.ActiveIndex = (int) index;
            // }
        }
    }
}