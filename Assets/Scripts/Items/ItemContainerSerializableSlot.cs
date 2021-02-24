using System;
using Items.ScriptableItems;
using ScriptableItems;
using UnityEngine;

namespace Items
{
    [Serializable] public struct ItemContainerSerializableSlot
    {
        [Min(0)] public int quantity;
        public ItemData data;
        public bool IsEmpty => quantity == 0 || data is null;
    }
}