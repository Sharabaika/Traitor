using UnityEngine;

namespace MapObjects
{
    [CreateAssetMenu(fileName = "New interactable obj style", menuName = "Interactable object style", order = 0)]
    public class InteractableObjectStyle : ScriptableObject
    {
        [SerializeField] private Color outlineOnHoverColor;
        [SerializeField, Range(0f, 10f)] private float outlineOnHoverWidth;

        [SerializeField] private Color outlineOnHoverWithSpecificToolColor;
        [SerializeField,Range(0f, 10f)] private float outlineOnHoverWithSpecificToolWidth;
        
        public Color OutlineOnHoverColor => outlineOnHoverColor;
        public float OutlineOnHoverWidth => outlineOnHoverWidth;
        public Color OutlineOnHoverWithSpecificToolColor => outlineOnHoverWithSpecificToolColor;
        public float OutlineOnHoverWithSpecificToolWidth => outlineOnHoverWithSpecificToolWidth;
    }
}