using UnityEngine;

namespace UserInterface.Inventory
{
    public class PlayerInventorySlotUI : ItemSlotUI
    {
        private bool _isOutlining = false;
        public bool IsOutlining
        {
            get => _isOutlining;
            set
            {
                _isOutlining = value;
                if (value)
                {
                    background.color = Color.red;
                }
                else
                {
                    background.color = Color.white;
                }
            }
        }
    }
}