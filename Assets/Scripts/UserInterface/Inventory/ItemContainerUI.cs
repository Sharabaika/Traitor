using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableItems;
using UnityEngine;

namespace UserInterface.Inventory
{
    [ExecuteInEditMode]public class ItemContainerUI : MonoBehaviour
    {
        [SerializeField] protected ItemContainer itemContainer;
        [SerializeField] private ItemSlotUI ItemSlotUIPrefab;
        [SerializeField] private Transform SlotsHolder;

        [SerializeField]private List<ItemSlotUI> _slotUIs = new List<ItemSlotUI>();
        
        [ContextMenu("Display slots")]public void DisplaySlots()
        {
            var deltaSlots = itemContainer.Capacity - _slotUIs.Count;

            if (deltaSlots < 0)
            {
                for (int i = 0; i < -deltaSlots; i++)
                {
                    var lastIndex = _slotUIs.Count - 1;
                    var slot = _slotUIs[lastIndex];
                    _slotUIs.RemoveAt(lastIndex);
                    StartCoroutine(DestroyAfterValidationFrame(slot.gameObject));
                }
            }
            else
            {
                for (int i = 0; i < _slotUIs.Count; i++)
                {
                    var slotUI = _slotUIs[i];
                    slotUI.ItemSlot = itemContainer.GetSlotByIndex(i);
                }
                
                for (int i = _slotUIs.Count; i < itemContainer.Capacity; i++)
                {
                    var slotUI = Instantiate(ItemSlotUIPrefab, Vector3.zero, Quaternion.identity, SlotsHolder);
                    slotUI.ItemContainer = itemContainer;
                    slotUI.ItemSlot = itemContainer.GetSlotByIndex(i);
                    _slotUIs.Add(slotUI);
                }
                UpdateSlots();
            }
        }

        [ContextMenu("Clear all ui slots")] public void ClearAll()
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

        private void Awake()
        {
            // DisplaySlots();
        }
    }
}