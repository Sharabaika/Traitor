using Characters;
using Items;
using ScriptableItems;
using UnityEngine;

namespace UserInterface.Inventory
{
    public class PlayerInventoryUI : ItemContainerUI
    {
        private PlayerInventorySlotUI _activeSlot;

        public void ShowInventoryForPlayer(PlayerCharacter character)
        {
            itemContainer = character.GetComponent<ItemContainer>();
            itemContainer.onItemsUpdated.AddListener(UpdateSlots);
            itemContainer.onInventoryReshape.AddListener(DisplaySlots);
            itemContainer.onItemsSynchronized.AddListener(UpdateSlots);
            (itemContainer as PlayerInventory)?.OnActiveSlotChanged.AddListener(ChangeActiveSlot);
            DisplaySlots();
        }
        
        public void ChangeActiveSlot(int index)
        {
            if (_activeSlot != null)
                _activeSlot.IsOutlining = false;
            if (index >= 0)
            {
                _activeSlot = (PlayerInventorySlotUI)slotUIs[index];
                _activeSlot.IsOutlining = true;
            }
        }
    }
}