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
        [SerializeField] private PlayerInventory inventory;

        public void SendActiveItemSlot(int index)
        {
            if(!photonView.IsMine)
                return;

            var hash = new Hashtable {{"ActiveSlot", index}};
            photonView.Owner.SetCustomProperties(hash);
        }
        public void SendItems()
        {
            if(!photonView.IsMine)
                return;

            var hash = new Hashtable();
            
            for (var i = 0; i < inventory.Capacity; i++)
            {
                var slot = inventory.GetSlotByIndex(i);

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
            if(PhotonNetwork.LocalPlayer == photonView.Owner)
                return;

            Debug.Log(inventory);
            Debug.Log(inventory.Capacity);
            
            // inventory slots
            var needToUpdateInventory = false;
            for (var i = 0; i < inventory.Capacity; i++)
            {
                if(changedProps.TryGetValue("SlotQuantity"+i.ToString(),  out var quantity))
                {
                    var slot = inventory.GetSlotByIndex(i);
                    slot.Quantity = (int) quantity;
                    needToUpdateInventory = true;
                }

                if(changedProps.TryGetValue("SlotItem"+i.ToString(),  out var ID))
                {
                    var slot = inventory.GetSlotByIndex(i);
                    slot.ItemInstance = itemList.GetItemData((int)ID).GetItemInstance();
                    needToUpdateInventory = true;
                }

                if(changedProps.TryGetValue("SlotState"+i.ToString(),  out var state))
                {
                    var slot = inventory.GetSlotByIndex(i);
                    slot.ItemInstance.DeserializeState((string) state);
                    needToUpdateInventory = true;
                }
            }
            if(needToUpdateInventory)
                inventory.onItemsSynchronized?.Invoke();
            
            // Active slot
            if (changedProps.TryGetValue("ActiveSlot", out var index))
            {
                inventory.ActiveIndex = (int)index;
            }
        }
    }
}