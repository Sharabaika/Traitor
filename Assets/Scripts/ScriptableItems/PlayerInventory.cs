using UnityEngine;
using UnityEngine.Events;

namespace ScriptableItems
{
    public class PlayerInventory : ItemContainer
    {
        [SerializeField] public UnityEvent<ItemSlot> OnActiveItemChanged;
        [SerializeField] public UnityEvent<int> OnActiveSlotChanged;

        private int _activeIndex;
        public ItemSlot ActiveSlot => _itemSlots[_activeIndex];
        public int ActiveIndex
        {
            get => _activeIndex;
            set
            {
                _activeIndex = value;
                OnActiveSlotChanged.Invoke(value);
                OnActiveItemChanged.Invoke(ActiveSlot);
            }
        }

        private void Update()
        {
            if(!photonView.IsMine)
                return;
            
            for (int i = 0; i < 5; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    ActiveIndex = i;
                    break;
                }
            }
        }
    }
}