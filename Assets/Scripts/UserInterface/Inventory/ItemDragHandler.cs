using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UserInterface.Inventory
{
    [ExecuteInEditMode]
    public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected ItemSlotUI itemSlotUi;

        private CanvasGroup _canvasGroup;
        private Transform _originalParent;
        private bool isHovering = false;

        public ItemSlotUI ItemSlotUi => itemSlotUi;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnDisable()
        {
            if (isHovering)
                isHovering = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown");
            if (eventData.button == PointerEventData.InputButton.Left)
            {

                _originalParent = transform.parent;
                transform.SetParent(transform.parent.parent);
                _canvasGroup.blocksRaycasts = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Debug.Log("OnDrag");
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                transform.position = Input.mousePosition;
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("OnPointerUp");

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                transform.SetParent(_originalParent);
                transform.localPosition = Vector3.zero;
                _canvasGroup.blocksRaycasts = true;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter");

            isHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPointerExit");

            isHovering = false;
        }
    }
}