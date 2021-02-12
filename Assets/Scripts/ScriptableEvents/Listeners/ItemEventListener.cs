using ScriptableEvents.Events;
using ScriptableItems;
using UnityEngine.Events;

namespace ScriptableEvents.Listeners
{
    public class ItemEventListener : ScriptableEventListener<ItemData, ItemEvent, UnityEvent<ItemData>> { }
}