using UnityEngine;
using UnityEngine.UI;

namespace Character
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