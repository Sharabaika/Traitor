using System;
using Items.ItemInstances;
using ScriptableItems;
using UnityEditor.UIElements;
using UnityEngine;

namespace Networking.Items
{
    [CreateAssetMenu(fileName = "New ItemList", menuName = "Networking/Items/ItemList", order = 0)]
    public class ItemList : ScriptableObject
    {
        [SerializeField] private ItemData[] _items;

        public ItemData GetItemData(int ID)
        {
            return ID > 0 ? _items[ID-1] : null;
        }

        public int GetItemDataID(ItemData itemData)
        {
            if (itemData is null)
                return 0;
            return Array.IndexOf(_items, itemData)+1;
        }

        public ItemInstance GetItemInstance(int id)
        {
            return GetItemData(id).GetItemInstance();
        }

        public int GetItemDataID(ItemInstance instance)
        {
            if (instance is null || instance.Data is null)
                return 0;
            return GetItemDataID(instance.Data);
        }
    }
}