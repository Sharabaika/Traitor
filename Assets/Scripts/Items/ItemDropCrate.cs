using System.Linq;
using System.Text;
using Items.ItemInstances;
using Photon.Pun;
using UnityEngine;

namespace Items
{
    public class ItemDropCrate : ItemContainer
    {
        // TODO add destroying effects
        protected override void OnItemsUpdated()
        {
            base.OnItemsUpdated();
            HandleSize();
            DestroyIfEmpty();
        }

        protected override void OnItemsSynchronized()
        {
            DestroyIfEmpty();
        }

        public override void Add(ItemSlot itemsToAdd)
        {
            base.Add(itemsToAdd);
            
            if (itemsToAdd.IsEmpty == false)
            {
                Resize(Capacity+1);
                var lastSlot = itemSlots[Capacity-1];
                ItemSlot.SwapItems(lastSlot, itemsToAdd);
            }
            onItemsUpdated.Invoke();
            
        }

        public void HandleSize()
        {
            for (int i = 0; i < Capacity-1; i++)
            {
                var current = itemSlots[i];
                var next = itemSlots[i + 1];
                if (current.IsEmpty && next.IsEmpty == false)
                {
                    ItemSlot.SwapItems(current, next);
                }
            }

            var occupied = itemSlots.Count(slot => !slot.IsEmpty);
            Resize(occupied+1);
        }


        public void DestroyIfEmpty()
        {
            if (photonView.IsMine && itemSlots.All(slot => slot.IsEmpty))
            {
                PhotonNetwork.Destroy(photonView);
            } 
        }

        public static ItemDropCrate CreateItemPickup(Vector3 pos, Quaternion rot)
        {
            var obj = PhotonNetwork.Instantiate("ItemPickup", pos, rot);
            return obj.GetComponent<ItemDropCrate>();
        }
    }
}