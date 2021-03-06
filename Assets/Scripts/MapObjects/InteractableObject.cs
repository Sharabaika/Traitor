using Characters;
using Logics;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace MapObjects
{
    [RequireComponent(typeof(Collider))]
    public class InteractableObject: MonoBehaviourPunCallbacks
    {
        [SerializeField] private string interactionText;

        [SerializeField] private UnityEvent<PlayerCharacter> onUsedByCharacterLocal;
        [SerializeField] private UnityEvent<PlayerCharacter> onUsedByCharacterSync;
        [SerializeField] private UnityEvent<PlayerCharacter> onLooseInterestLocal;
        [SerializeField] private bool isOutliningOnMouseOver = true;

        [SerializeField] private InteractableObjectStyle style;
        
        
        private bool _isOutlining = false;
        private bool IsOutlining
        {
            get => _isOutlining;
            set
            {
                _isOutlining = value;
                // _renderer.sharedMaterial = value ? style.outlineShader : style.defaultShader;
            }
        }

        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        public virtual void Interact(PlayerCharacter with)
        {
            onUsedByCharacterLocal?.Invoke(with);
            photonView.RPC("SyncInteraction", RpcTarget.All, with.photonView.Owner);
        }

        public virtual void StopInteracting(PlayerCharacter with)
        {
            onLooseInterestLocal?.Invoke(with);
        }

        [PunRPC] public void SyncInteraction(Player interactor)
        {
            var character = GameManager.Instance.Characters[interactor];
            DefaultInteraction(character);
            onUsedByCharacterSync?.Invoke(character);
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

        protected virtual void OnNewPlayerJoinedRoom(Player newPlayer)
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

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            OnNewPlayerJoinedRoom(newPlayer);
        }
    }
}