using Characters;
using ScriptableItems;
using UnityEngine;

namespace ScriptableEvents.Events
{
    [CreateAssetMenu(fileName = "New Character Event", menuName = "Game Events/Cahracter Event")]
    public class PlayerCharacterEvent : BaseGameEvent<PlayerCharacter> { }
}