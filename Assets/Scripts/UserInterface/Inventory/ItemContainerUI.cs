using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableItems;
using UnityEngine;

namespace UserInterface.Inventory
{
    [ExecuteAlways] public class ItemContainerUI : MonoBehaviour
    {
        [SerializeField] protected ItemContainer itemContainer;
        [SerializeField] private ItemSlotUI ItemSlotUIPrefab;
        [SerializeField] private Transform SlotsHolder;
        [SerializeField] private Canvas firstPriorityCanvas;

        public Canvas FirstPriorityCanvas => firstPriorityCanvas;

        protected List<ItemSlotUI> _slotUIs = new List<ItemSlotUI>();

        [ContextMenu("Display slots")]
        public void DisplaySlots()
        {
            if(itemContainer is null)
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