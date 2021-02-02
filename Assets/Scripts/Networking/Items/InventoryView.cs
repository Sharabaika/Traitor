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
                var id = itemList.GetItemID(slot.Item);
                var quantity = slot.Quantity;
                
                hash.Add("SlotItem"+i.ToString(), id);
                hash.Add("SlotQuantity" + i.ToString(), quantity);
            }

            photonView.Owner.SetCustomProperties(hash);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if(targetPlayer != photonView.Owner)
                return;
            if(PhotonNetwork.LocalPlayer == photonView.Owner)
                return;

            // inventory slots
            var needToUpdateInventory = false;
            for (var i = 0; i < inventory.Capacity; i++)
            {
                if(changedProps.TryGetValue("SlotItem"+i.ToString(),  out var ID))
                {
                    var slot = inventory.GetSlotByIndex(i);
                    slot.Item = itemList.GetItemByID((int)ID);
                    needToUpdateInventory = true;
                }
                
                if(changedProps.TryGetValue("SlotQuantity"+i.ToString(),  out var quantity))
                {
                    var slot = inventory.GetSlotByIndex(i);
                    slot.Quantity = (int) quantity;
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