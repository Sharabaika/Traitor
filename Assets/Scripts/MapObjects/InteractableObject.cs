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

        [SerializeField] private InteractableObjectStyle outlineStyle;
        
        public UnityEvent<PlayerCharacter> onStopInteractingLocal;

        private OutlineGroup _outlineGroup;

        public virtual void Interact(PlayerCharacter with)
        {
            onUsedByCharacterLocal?.Invoke(with);
            photonView.RPC("SyncInteraction", RpcTarget.All, with.photonView.Owner);
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

        public void HighlightObject(PlayerCharacter interactor)
        {
            _outlineGroup.GroupColor = outlineStyle.OutlineOnHoverColor;
            _outlineGroup.GroupWidth = outlineStyle.OutlineOnHoverWidth;
            _outlineGroup.GroupEnabled = true;
        }

        public void StopHighlighting()
        {
            _outlineGroup.GroupEnabled = false;
        }
        

        private void Awake()
        {
            _outlineGroup = GetComponent<OutlineGroup>();
            _outlineGroup.GroupEnabled = false;
            OnAwake();
        }

        private void Start()
        {
            OnStart();
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

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            OnNewPlayerJoinedRoom(newPlayer);
        }
    }
}