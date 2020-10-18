using System;
using Characters;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace MapObjects
{
    public class InteractableObject: MonoBehaviour
    {
        [SerializeField] private string interactionText;

        [SerializeField] private UnityEvent<PlayerCharacter> OnUsedByCharacter;
        [SerializeField] private bool isOutliningOnMouseOver = true;

        [SerializeField] private Material outlineShader;
        [SerializeField] private Material defaultShader;

        private SpriteRenderer _renderer;
        
        private bool _isOutlining = false;
        private bool IsOutlining
        {
            get => _isOutlining;
            set
            {
                _isOutlining = value;
                _renderer.sharedMaterial = value ? outlineShader : defaultShader;
            }
        }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public virtual void Interact(PlayerCharacter with)
        {
            OnUsedByCharacter?.Invoke(with);
        }

        private void OnMouseEnter()
        {
            IsOutlining = true;
        }

        private void OnMouseExit()
        {
            IsOutlining = false;
        }
    }
}