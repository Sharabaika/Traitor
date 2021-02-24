using System;
using Items.ItemInstances;
using Items.ScriptableItems;
using ScriptableItems;
using UnityEditor.UIElements;
using UnityEngine;

namespace Networking.Items
{
    [CreateAssetMenu(fileName = "New ItemList", menuName = "Networking/ItemList", order = 0)]
    public class ItemList : IDList<ItemData>
    {
        public ItemInstance GetItemInstance(int id)
        {
            return GetItem(id).GetItemInstance();
        }

        public int GetItemDataID(ItemInstance instance)
        {
            if (instance is null || instance.Data is null)
                return 0;
            return GetID(instance.Data);
        }
    }
}