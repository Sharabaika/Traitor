using System;
using Items.ItemInstances;
using UnityEngine;

namespace Networking
{
    public abstract class IDList<T> : ScriptableObject where T:class
    {
        [SerializeField] protected T[] items;

        public T GetItem(int ID)
        {
            return ID > 0 ? items[ID-1] : (T) null;
        }

        public int GetID(T itemData)
        {
            if (itemData is null)
                return 0;
            return Array.IndexOf(items, itemData)+1;
        }
    }
}