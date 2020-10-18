using UnityEngine;

namespace Multiplayer
{
    [CreateAssetMenu(fileName = "New player account", menuName = "New player", order = 0)]
    public class PlayerAccount : ScriptableObject
    {
        // TODO rename class
        
        public string NickName = "Player";
        
        // TODO add color
    }
}