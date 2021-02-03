using ScriptableItems;
using UnityEngine;

namespace ScriptableEvents.Events
{
    [CreateAssetMenu(fileName = "New Item Event", menuName = "Game Events/Item Event")]
    public class ItemEvent : BaseGameEvent<Item> { }
}