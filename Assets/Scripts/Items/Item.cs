using Characters;
using Items.ItemInstances;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class Item : MonoBehaviour
    { 
        [SerializeField] protected GameObject model;
        [SerializeField] protected UnityEvent OnUse;
        
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
        
        public ItemInstance RepresentationOf { get; set; }
        
        public virtual void Use(PlayerCharacter by)
        {
            OnUse?.Invoke();
            Debug.Log(by + " used " + gameObject.name);
        }

        public virtual void HandlePositioning(PlayerCharacter owner)
        {
        }
    }
}