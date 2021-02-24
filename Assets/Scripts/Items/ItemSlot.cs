using Items.ItemInstances;
using UnityEngine;

namespace Items.ScriptableItems
{
    public class ItemSlot
    {
        private ItemInstance _itemInstance;
        private int _quantity = 0;
        
        public ItemInstance ItemInstance
        {
            get => _itemInstance;
            set
            {
                _itemInstance = value;
                if (!value.IsNull)
                {
                    _quantity = Mathf.Clamp(_quantity, 0, value.Data.MaxStack);
                }
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (ItemInstance.IsNull)
                {
                    _quantity = value;
                }
                else
                {
                    if (value < 1)
                    {
                        ItemInstance = new ItemInstance();
                        _quantity = 0;
                    }
                    else
                    {
                        _quantity = Mathf.Min(value, ItemInstance.Data.MaxStack);
                    }
                }
            }
        }

        public bool IsEmpty => Quantity == 0;

        public ItemSlot()
        {
            ItemInstance = new ItemInstance();
            _quantity = 0;
        }
        
        public ItemSlot(int quantity, ItemInstance item)
        {
            _itemInstance = item;
            _quantity = quantity;
        }

        public static void SwapItems(ItemSlot a, ItemSlot b)
        {
            var tItem = a._itemInstance;
            var tQuantity = a._quantity;
            a.ItemInstance = b.ItemInstance;
            a.Quantity = b.Quantity;
            b.ItemInstance = tItem;
            b.Quantity = tQuantity;
        }
    }
}