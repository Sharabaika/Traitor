using System;
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

        private Item _itemRepresentation;
        public ItemInstance(ItemData data = null)
        {
            Data = data;
        }

        
        // TODO add UseOn( by, target)
        public virtual void UseBy(PlayerCharacter user)
        {
            
        }

        public virtual void AlternativeUse(PlayerCharacter user)
        {
            
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