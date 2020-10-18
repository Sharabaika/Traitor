using UnityEngine;

namespace Misc
{
    public class HealthPanel : MonoBehaviour
    {
        [SerializeField] private TextMesh text;

        public void ChangeRemainingHealth(float health)
        {
            text.text = Mathf.CeilToInt(health).ToString();
        }
    }
}