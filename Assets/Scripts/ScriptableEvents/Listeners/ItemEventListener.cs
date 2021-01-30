using DapperDino.Events.CustomEvents;
using DapperDino.Events.Listeners;
using ScriptableItems;
using UnityEngine.Events;

namespace ScriptableEvents.Listeners
{
    public class ItemEventListener : ScriptableEventListener<Item, ItemEvent, UnityEvent<Item>> { }
}