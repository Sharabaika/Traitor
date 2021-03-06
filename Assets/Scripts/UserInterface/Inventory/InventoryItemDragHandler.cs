using Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UserInterface.Inventory
{
    public class InventoryItemDragHandler: ItemDragHandler
    {
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                base.OnPointerUp(eventData);
            }

            if (eventData.hovered.Count == 0)
            {
                // drop item
                var pos = itemSlotUi.ItemContainer.transform.position;
                var pickup = ItemDropCrate.CreateItemPickup(pos, Quaternion.identity);
                pickup.Add(ItemSlotUi.ItemSlot);
                itemSlotUi.ItemContainer.onItemsUpdated.Invoke();
            }
        }
    }
}