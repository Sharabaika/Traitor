using ScriptableEvents;
using ScriptableEvents.Events;
using ScriptableEvents.Listeners;
using UnityEngine.Events;

namespace DapperDino.Events.Listeners
{
    public class VoidListener : ScriptableEventListener<Void, VoidEvent, UnityEvent<Void>> { }
}