using Characters;
using Items.ItemInstances;
using Logics;
using Photon.Pun;
using UnityEngine;

namespace Items.ItemRepresentations
{
    public class Item : MonoBehaviourPun, IPunObservable
    { 
        [SerializeField] protected GameObject model;

        protected ItemInstance itemInstance;
        private PlayerCharacter _owner;

        protected bool OwnerIsLocal { get; private set; }
        
        public bool HasOwner { get; private set; }
        
        protected PlayerCharacter Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                HasOwner = value != null;
            }
        }

        private bool _isHidden = false;
        public bool isHidden
        {
            get => _isHidden;
            set
            {
                _isHidden = value; 
                model.SetActive(!value);
            }
        }

        public virtual void SetItemInstance(ItemInstance instance)
        {
            itemInstance = instance;
        }
        
        
        public virtual void Use(PlayerCharacter by){}
        public virtual void AlternativeUse(PlayerCharacter by){}

        private void Update()
        {
            OnUpdate();
        }
        
        protected virtual void OnUpdate(){}

        private void Awake()
        {
            var owner = GameManager.Instance.Characters[photonView.Owner];
            transform.SetParent(owner.Inventory.AnchorTransform);
        }

        public virtual void HandlePositioning(PlayerCharacter ownerCharacter)
        {
            Owner = ownerCharacter;
            OwnerIsLocal = ownerCharacter.photonView.Owner.IsLocal;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(isHidden);
            }
            else
            {
                isHidden = (bool) stream.ReceiveNext();
            }
        }
    }
}