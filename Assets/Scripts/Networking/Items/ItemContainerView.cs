using System.Text;
using ExitGames.Client.Photon;
using Items.ScriptableItems;
using Photon.Pun;
using Photon.Realtime;
using ScriptableItems;
using UnityEngine;

namespace Networking.Items
{
    public class ItemContainerView : MonoBehaviourPunCallbacks
    {
        [SerializeField] private ItemList itemList;
        [SerializeField] private ItemContainer container;

        private string GetKey()=> "ItemContainer"+photonView.ViewID.ToString();

        public void SendItems()
        {
            var builder = new StringBuilder();

            var key = GetKey();
            
            for (var i = 0; i < container.Capacity; i++)
            {
                var slot = container.GetSlotByIndex(i);
                builder.Append(SerializeSlot(slot)).Append(' ');
            }

            var hash = new Hashtable {{key, builder.ToString()}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.TryGetValue(GetKey(), out var data))
            {
                var str = (string) data;
                var slotsData = str.TrimEnd().Split(' ');

                var i = 0;
                foreach (var slotData in slotsData)
                {
                    var containerSlot = container.GetSlotByIndex(i);
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
                container.onItemsSynchronized?.Invoke();
            }
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
    }
}