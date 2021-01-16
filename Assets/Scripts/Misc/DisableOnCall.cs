using UnityEngine;

namespace Misc
{
    public class DisableOnCall : MonoBehaviour
    {
        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}