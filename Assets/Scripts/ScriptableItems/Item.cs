using UnityEngine;

namespace ScriptableItems
{
    [CreateAssetMenu(fileName = "new item", menuName = "Items/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private new string name = "item name";
        [SerializeField] private Sprite icon;
        [SerializeField,Min(1)] private int maxStack = 1;
        [SerializeField] private string description;

        public string Name => name;
        public Sprite Icon => icon;
        public int MaxStack => maxStack;
        public string Description => description; 
    }
}