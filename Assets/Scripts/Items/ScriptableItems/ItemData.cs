using System;
using Items.ItemInstances;
using Items.ItemRepresentations;
using UnityEditor;
using UnityEngine;

namespace Items.ScriptableItems
{
    [CreateAssetMenu(fileName = "new item", menuName = "Items/Item")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField,Min(1)] private int maxStack = 1;
        [SerializeField] private string description;
        [SerializeField] private Item item;
        
        // Костыль, но работает
        [SerializeField, HideInInspector] private string path;
        public string PrefabPath
        {
            get => path;
            set => path = value;
        }
        
        public Sprite Icon => icon;
        public int MaxStack => maxStack;
        public string Description => description;
        public Item Item => item;
        public bool IsStackable => MaxStack != 1;

        private void OnValidate()
        {
#if UNITY_EDITOR
            path = AssetDatabase.GetAssetPath(item)
                .Replace(".prefab", "")
                .Replace("Assets/Resources/", "");
#endif
        }

        public virtual ItemInstance GetItemInstance()
        {
            return new ItemInstance(this);
        }
    }
}