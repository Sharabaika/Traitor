using System;
using ScriptableItems;
using UnityEditor.UIElements;
using UnityEngine;

namespace Networking.Items
{
    [CreateAssetMenu(fileName = "New ItemList", menuName = "Networking/Items/ItemList", order = 0)]
    public class ItemList : ScriptableObject
    {
        [SerializeField] private Item[] _items;

        public Item GetItemByID(int ID)
        {
            return ID > -1 ? _items[ID] : null;
        }

        public int GetItemID(Item item)
        {
            if (item is null)
                return -1;
            return Array.IndexOf(_items, item);
        }
    }
}