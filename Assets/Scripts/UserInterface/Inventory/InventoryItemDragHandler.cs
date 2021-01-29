using UnityEngine;
using UnityEngine.EventSystems;

namespace UserInterface.Inventory
{
    public class InventoryItemDragHandler: ItemDragHandler
    {
        public override void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("OnPointerUp InventoryItemDragHandler");
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                base.OnPointerUp(eventData);
            }

            if (eventData.hovered.Count == 0)
            {
                // drop item
            }
        }
    }
}