using UnityEngine;

namespace Multiplayer
{
    [CreateAssetMenu(fileName = "New player account", menuName = "New player", order = 0)]
    public class PlayerPreferences : ScriptableObject
    {
        public string nickName = "Player";
    }
}