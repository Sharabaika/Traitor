using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableItems;
using UnityEngine;

namespace UserInterface.Inventory
{
    // TODO fix scene updating
    public class ItemContainerUI : MonoBehaviour
    {
        [SerializeField] protected ItemContainer itemContainer;
        [SerializeField] private ItemSlotUI ItemSlotUIPrefab;
        [SerializeField] private Transform SlotsHolder;
        [SerializeField] private Canvas canvas;
        
        private Canvas _firstPriorityCanvas;
        public Canvas FirstPriorityCanvas => _firstPriorityCanvas;

        [SerializeField]protected List<ItemSlotUI> _slotUIs = new List<ItemSlotUI>();

        public void Show()
        {
            canvas.gameObject.SetActive(true);   
            DisplaySlots();
        }

        public void Hide()
        {
            canvas.gameObject.SetActive(false);
        }
        

        [ContextMenu("Display slots")]
        public void DisplaySlots()
        {
            if (FirstPriorityCanvas is null)
            {
                _firstPriorityCanvas = FindObjectOfType<FirstPriorityCanvas>().GetComponent<Canvas>();
            }
            
            if(itemContainer is null)
                return;
            if(SlotsHolder is null)
                return;

            ClearAll();

            for (int i = 0; i < itemContainer.Capacity; i++)
            {
                var slotUI = Instantiate(ItemSlotUIPrefab, Vector3.zero, Quaternion.identity, SlotsHolder);
                slotUI.ItemContainer = itemContainer;
                slotUI.ItemSlot = itemContainer.GetSlotByIndex(i);
                slotUI.ItemContainerUI = this;
                _slotUIs.Add(slotUI);
            }

            UpdateSlots();
        }

        private void ClearAll()
        {
            foreach (Transform child in SlotsHolder)
            {
                StartCoroutine(DestroyAfterValidationFrame(child.gameObject));
            }

            _slotUIs.Clear();
        }

        public void UpdateSlots()
        {
            foreach (var ui in _slotUIs)
            {
                ui.UpdateSlotUI();
            }
        }

        private IEnumerator DestroyAfterValidationFrame(GameObject obj)
        {
            yield return null;
            DestroyImmediate(obj);
        }
    }
}