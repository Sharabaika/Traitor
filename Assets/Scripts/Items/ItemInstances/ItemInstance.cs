using System;
using System.IO;
using Characters;
using Items.ScriptableItems;
using Misc;
using ScriptableItems;
using UnityEngine;

namespace Items.ItemInstances
{
    public class ItemInstance
    {
        public ItemData Data { get; }
        public bool IsNull => Data is null;

        public ItemInstance(ItemData data = null)
        {
            Data = data;
        }

        public virtual string GetStatus()
        {
            return "";
        }

        public virtual string SerializeState()
        {
            return "";
        }

        public virtual void DeserializeState(string data)
        {
            
        }

        public static bool CanStack(ItemInstance a, ItemInstance b)
        {
            if (a.IsNull || b.IsNull)
                return false;
            if (a.Data == b.Data)
                return true;
            return false;
        }
    }
}