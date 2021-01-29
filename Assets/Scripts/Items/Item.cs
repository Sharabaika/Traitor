using Characters;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class Item : MonoBehaviourPun
    {
        
        
        [SerializeField] protected GameObject sprite;
        [SerializeField] protected UnityEvent OnUse;
        [SerializeField] protected bool isSync = true;
        
        private bool _isHidden = false;
        public bool isHidden
        {
            get => _isHidden;
            set
            {
                _isHidden = value; 
                sprite.SetActive(!value);
            }
        }

        [PunRPC]public virtual void SyncInteraction()
        {
            OnUse?.Invoke();
        }
        
        public virtual void Use(PlayerCharacter by)
        {
            if (isSync)
            {
                photonView.RPC("SyncInteraction", RpcTarget.Others);
            }
            Debug.Log(by + " used " + gameObject.name);
        }
    }
}