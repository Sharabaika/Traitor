﻿using System;
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
        
        public virtual void Use(PlayerCharacter by)
        {
            OnUse?.Invoke();
        }

        private void Update()
        {
            OnUpdate();
        }
        
        protected virtual void OnUpdate(){}

        public virtual void HandlePositioning(PlayerCharacter ownerCharacter)
        {
            Owner = ownerCharacter;
            OwnerIsLocal = ownerCharacter.photonView.Owner.IsLocal;
        }
    }
}