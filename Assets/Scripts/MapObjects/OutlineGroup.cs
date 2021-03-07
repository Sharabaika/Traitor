using System;
using UnityEngine;

namespace MapObjects
{
    public class OutlineGroup : MonoBehaviour
    {
        [SerializeField] private Outline[] outlines;

        private Color _color;
        public Color GroupColor
        {
            get => _color;
            set
            {
                foreach (var outline in outlines)
                {
                    outline.OutlineColor = value;
                }
            }
        }
        
        private float _width;
        public float GroupWidth
        {
            get => _width;
            set
            {
                foreach (var outline in outlines)
                {
                    outline.OutlineWidth = value;
                }
            }
        }

        private bool _enabled;
        public bool GroupEnabled
        {
            get => _enabled;
            set
            {
                foreach (var outline in outlines)
                {
                    outline.enabled = value;
                }
            }
        }
    }
}