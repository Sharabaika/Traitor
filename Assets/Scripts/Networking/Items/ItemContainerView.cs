using System.Text;
using ExitGames.Client.Photon;
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
                var id = itemList.GetItemID(slot.Item);
                var quantity = slot.Quantity;

                builder.Append(id).Append(":").Append(quantity).Append(" ");
            }

            var hash = new Hashtable {{key, builder.ToString()}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.TryGetValue(GetKey(), out var data))
            {
                var str = (string) data;
                Debug.Log(str);
                var slots = str.TrimEnd().Split(' ');

                var i = 0;
                foreach (var slot in slots)
                {
                    var parsedSlot = slot.Split(':');
                    var containerSlot = container.GetSlotByIndex(i);
                    
                    containerSlot.Item =itemList.GetItemByID(int.Parse(parsedSlot[0]));
                    containerSlot.Quantity = int.Parse(parsedSlot[1]);
                    i++;
                }
                
                container.onItemsSynchronized?.Invoke();

            }
        }
    }
}