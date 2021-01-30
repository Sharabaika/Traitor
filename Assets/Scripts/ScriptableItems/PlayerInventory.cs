using UnityEngine;
using UnityEngine.Events;

namespace ScriptableItems
{
    public class PlayerInventory : ItemContainer
    {
        [SerializeField] private UnityEvent<ItemSlot> OnActiveSlotChanged;
        
        
        private ItemSlot _activeSlot;
        public ItemSlot ActiveSlot
        {
            get => _activeSlot;
            set
            {
                if(_activeSlot == value)
                    return;
                OnActiveSlotChanged.Invoke(value);
                _activeSlot = value;
            }
        }
    }
}