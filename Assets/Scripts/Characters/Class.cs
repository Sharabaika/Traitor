using Items;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "New Class", menuName = "Character Class/Class", order = 0)]
    public class Class : ScriptableObject
    {
        [SerializeField] private ItemContainerSerializableSlot[] startingInventory;
        [SerializeField] private int maxHealth;
        [SerializeField] private int startingHealth;
        
        public ItemContainerSerializableSlot[] StartingInventory => startingInventory;
        public int MaxHealth => maxHealth;
        public int StartingHealth => startingHealth;
        
        // TODO add specific features
    }
}