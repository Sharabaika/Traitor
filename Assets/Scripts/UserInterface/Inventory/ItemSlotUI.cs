using ScriptableItems;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UserInterface.Inventory
{
    [ExecuteInEditMode] public class ItemSlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI itemQuantityText;

        // public ItemSlot ItemSlot => itemContainer.GetSlotByIndex(_slotIndex);

        [SerializeField]public ItemContainer ItemContainer;

        [SerializeField]private ItemSlot _itemSlot;
        public ItemSlot ItemSlot
        {
            get => _itemSlot;
            set => _itemSlot = value;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
            if (itemDragHandler is null)
                return;
            ItemContainer.Combine(itemDragHandler.ItemSlotUi.ItemSlot, ItemSlot);
        }

        public void UpdateSlotUI()
        {
            if (ItemSlot.IsEmpty)
            {
                EnableSlotUI(false);
                return;
            }
            EnableSlotUI(true);
            icon.sprite = ItemSlot.Item.Icon;
            itemQuantityText.text = ItemSlot.Quantity.ToString();
        }

        private void EnableSlotUI(bool enable)
        {
            icon.enabled = enable;
            itemQuantityText.enabled = enable;
        }
    }
}