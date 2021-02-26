using UnityEngine;

namespace Multiplayer
{
    [CreateAssetMenu(fileName = "NewPlayerPrefs", menuName = "Player Preferences", order = 0)]
    public class PlayerPreferences : ScriptableObject
    {
        public string nickName = "Player";
    }
}