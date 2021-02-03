using Characters;
using ScriptableEvents.Events;
using ScriptableItems;
using UnityEngine.Events;

namespace ScriptableEvents.Listeners
{
    public class PlayerCharacterEventListener : ScriptableEventListener<PlayerCharacter, PlayerCharacterEvent, UnityEvent<PlayerCharacter>> { }
}