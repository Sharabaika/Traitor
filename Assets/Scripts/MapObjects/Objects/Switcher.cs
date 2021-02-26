using System;
using Characters;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace MapObjects.Objects
{
    public class Switcher : InteractableObject
    {
        [SerializeField] public UnityEvent onActivation;
        [SerializeField] public UnityEvent onDeactivate;

        [SerializeField] private bool On;
        
        private bool _isOn;
        public bool isOn
        {
            get => _isOn;
            set
            {
                if (isOn == value) return;
                _isOn = value;
                TriggerEvents(isOn);
            }
        }

        [PunRPC] public void SyncState(bool state, bool triggerEvents)
        {
            _isOn = state;
            On = state;
            if (triggerEvents)
                TriggerEvents(state);
        }

        private void TriggerEvents(bool isOn)
        {
            if (isOn)
            {
                onActivation?.Invoke();
            }
            else
            {
                onDeactivate?.Invoke();
            }
        }

        protected override void DefaultInteraction(PlayerCharacter player)
        {
            Switch();
        }

        public void Switch()
        {
            Debug.Log(gameObject.name + " switcher is switched");
            isOn = !isOn;
        }

        private void OnValidate()
        {
            // isOn = On;
        }

        protected override void OnNewPlayerJoinedRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("SyncState", newPlayer, isOn, true);
            }
        }
    }
}