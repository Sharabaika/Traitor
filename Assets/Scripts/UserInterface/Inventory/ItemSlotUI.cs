using Items;
using Items.ScriptableItems;
using ScriptableItems;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UserInterface.Inventory
{
    public class ItemSlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] protected Image background;
        [SerializeField] protected Image icon;
        
        /// <summary>
        /// item quantity or status field
        /// </summary>
        [SerializeField] protected TextMeshProUGUI itemStatusTextfield;


        // public ItemContainer ItemContainer;
        public ItemContainer ItemContainer { get; set; }
        public ItemSlot ItemSlot { get; set; }
        public ItemContainerUI ItemContainerUI { get; set; }

        public void OnDrop(PointerEventData eventData)
        {
            var itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
            if (itemDragHandler is null)
                return;
            var dropped = itemDragHandler.ItemSlotUi;
            ItemContainer.Combine(dropped.ItemSlot, ItemSlot);

            // TODO the only way to shift items between chests
            if (ItemContainer != dropped.ItemContainer)
            {
                dropped.ItemContainer.onItemsUpdated.Invoke();
            }
        }

        public void UpdateSlotUI()
        {
            if (ItemSlot.IsEmpty)
            {
                EnableSlotUI(false);
                return;
            }
            EnableSlotUI(true);
            icon.sprite = ItemSlot.ItemInstance.Data.Icon;
            
            if(ItemSlot.ItemInstance.Data.IsStackable)
            {
                itemStatusTextfield.text = ItemSlot.Quantity.ToString();
            }
            else
            {
                itemStatusTextfield.text = ItemSlot.ItemInstance.GetStatus();
            }
        }

        private void EnableSlotUI(bool enable)
        {
            icon.enabled = enable;
            itemStatusTextfield.enabled = enable;
        }
    }
}