using System;
using Characters;
using Logics;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UserInterface.InteractableObjectInterfaces;

namespace MapObjects
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractableObject: MonoBehaviourPunCallbacks
    {
        [SerializeField] private string interactionText;

        [SerializeField] private UnityEvent<PlayerCharacter> OnUsedByCharacter;
        [SerializeField] private bool isOutliningOnMouseOver = true;

        [SerializeField] private InteractableObjectStyle style;
        [SerializeField] protected InteractableObjectInterface ui;

        [SerializeField] private bool isSynchronized;
        
        private SpriteRenderer _renderer;
        
        private bool _isOutlining = false;
        private bool IsOutlining
        {
            get => _isOutlining;
            set
            {
                _isOutlining = value;
                _renderer.sharedMaterial = value ? style.outlineShader : style.defaultShader;
            }
        }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        public virtual void Interact(PlayerCharacter with)
        {
            if (ui != null)
            {
                ui.Display(with);
            }

            DefaultInteraction(with);
            OnUsedByCharacter?.Invoke(with);
            
            if(isSynchronized)
                photonView.RPC("Sync", RpcTarget.Others, with.photonView.Owner);
        }

        [PunRPC] public void Sync(Player interactor)
        {
            var character = GameManager.Instance.GetPlayersCharacter(interactor);
            DefaultInteraction(character);
            OnUsedByCharacter?.Invoke(character);
        }
        
        protected virtual void DefaultInteraction(PlayerCharacter player)
        {
            
        }
        
        protected virtual void OnAwake()
        {
            
        }
        
        protected virtual void OnStart()
        {
            
        }

        private void OnMouseEnter()
        {
            if(isOutliningOnMouseOver)
                IsOutlining = true;
        }

        private void OnMouseExit()
        {
            if(isOutliningOnMouseOver)
                IsOutlining = false;
        }
    }
}