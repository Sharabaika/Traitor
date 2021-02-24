using System.Text;
using Items.ScriptableItems;
using ScriptableItems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hel.Items
{
    public class ItemPopup : MonoBehaviour
    {
        [SerializeField] private GameObject popupCanvasObject = null;
        [SerializeField] private RectTransform popupObject = null;
        [SerializeField] private TextMeshProUGUI infoText = null;
        [SerializeField] private Vector3 offset = new Vector3(0f, 50f, 0f);
        [SerializeField] private float padding = 25f;

        private Canvas popupCanvas = null;

        private void Start()
        {
            popupCanvas = popupCanvasObject.GetComponent<Canvas>();
            HideInfo();
        }

        private void Update() => FollowCursor();

        public void HideInfo() => popupCanvasObject.SetActive(false);

        private void FollowCursor()
        {
            if (!popupCanvasObject.activeSelf) { return; }

            Vector3 newPos = Input.mousePosition + offset;
            newPos.z = 0f;

            float rightEdgeToScreenEdgeDistance = Screen.width - (newPos.x + popupObject.rect.width * popupCanvas.scaleFactor / 2) - padding;
            if (rightEdgeToScreenEdgeDistance < 0)
            {
                newPos.x += rightEdgeToScreenEdgeDistance;
            }
            float leftEdgeToScreenEdgeDistance = 0 - (newPos.x - popupObject.rect.width * popupCanvas.scaleFactor / 2) + padding;
            if (leftEdgeToScreenEdgeDistance > 0)
            {
                newPos.x += leftEdgeToScreenEdgeDistance;
            }
            float topEdgeToScreenEdgeDistance = Screen.height - (newPos.y + popupObject.rect.height * popupCanvas.scaleFactor) - padding;
            if (topEdgeToScreenEdgeDistance < 0)
            {
                newPos.y += topEdgeToScreenEdgeDistance;
            }
            popupObject.transform.position = newPos;
        }

        public void DisplayInfo(ItemData infoItemData)
        {
            var builder = new StringBuilder();

            builder.Append("<size=75>").Append(infoItemData.name).Append("</size>\n");
            builder.Append(infoItemData.Description);

            infoText.text = builder.ToString();

            popupCanvasObject.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
        }
    }
}