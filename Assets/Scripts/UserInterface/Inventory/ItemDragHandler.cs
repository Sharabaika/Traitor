using System;
using DapperDino.Events.CustomEvents;
using ScriptableEvents.Events;
using ScriptableItems;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UserInterface.Inventory
{
    [ExecuteInEditMode]
    public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected ItemSlotUI itemSlotUi;

        [SerializeField] protected ItemEvent OnStartHoveringItem;
        [SerializeField] protected VoidEvent OnStopHoveringItem;

        
        private CanvasGroup _canvasGroup;
        private Transform _originalParent;
        private bool _isHovering = false;

        public bool IsHovering
        {
            get => _isHovering;
            set
            {
                _isHovering = value;
                if(value == false)
                    OnStopHoveringItem.Raise();
            }
        }

        public ItemSlotUI ItemSlotUi => itemSlotUi;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnDisable()
        {
            if (_isHovering)
                _isHovering = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _originalParent = transform.parent;
                transform.SetParent(itemSlotUi.ItemContainerUI.FirstPriorityCanvas.transform);
                _canvasGroup.blocksRaycasts = false;
                IsHovering = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                transform.position = Input.mousePosition;
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                transform.SetParent(_originalParent);
                transform.localPosition = Vector3.zero;
                _canvasGroup.blocksRaycasts = true;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnStartHoveringItem.Raise(itemSlotUi.ItemSlot.Item);
            IsHovering = true;
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovering = false;
        }
    }
}