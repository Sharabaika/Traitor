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
        [SerializeField] private ItemContainer itemContainer;
        [SerializeField] private TextMeshProUGUI itemQuantityText;

        public ItemSlot ItemSlot => itemContainer.GetSlotByIndex(_slotIndex);

        private Item item { get; set; }
        private int _slotIndex;

        private void OnEnable()
        {
            _slotIndex = transform.GetSiblingIndex();
            UpdateSlotUi();
        }

        private void Start()
        {
            // TODO add OnLocalPlayerCreated to GameManager and listen here
            itemContainer.onItemsUpdated.AddListener(UpdateSlotUi);
        }

        public void OnDrop(PointerEventData eventData)
        {
            var itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
            if (itemDragHandler is null)
                return;
            itemContainer.Combine(ItemSlot, itemDragHandler.ItemSlotUi.ItemSlot);
        }

        private void UpdateSlotUi()
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