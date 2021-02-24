using Items.ScriptableItems;
using UnityEngine;

namespace ScriptableItems
{
    [CreateAssetMenu(fileName = "New consumable", menuName = "Itens/Consumable", order = 0)]
    public class ConsumableItemData : ItemData
    {
        [SerializeField] private int healing;
        [SerializeField] private bool isFood;
    }
}