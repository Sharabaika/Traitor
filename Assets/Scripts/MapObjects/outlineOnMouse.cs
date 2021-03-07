using System;
using UnityEngine;

namespace MapObjects
{
    public class outlineOnMouse : MonoBehaviour
    {
        private OutlineGroup _group;
        [SerializeField] private InteractableObjectStyle style;

        private void Awake()
        {
            _group = GetComponent<OutlineGroup>();
            _group.GroupEnabled = false;
        }

        private void OnMouseEnter()
        {
            _group.GroupEnabled = true;
        }

        private void OnMouseExit()
        {
            _group.GroupEnabled = false;
        }
    }
}