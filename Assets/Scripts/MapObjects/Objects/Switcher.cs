using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace MapObjects.Objects
{
    public class Switcher : MonoBehaviourPunCallbacks
    {
        [SerializeField] public UnityEvent onActivation;
        [SerializeField] public UnityEvent onDeactivate;

        [SerializeField] private bool On;

        [SerializeField] private bool isSync = true;
        
        private bool _isOn;
        public bool isOn
        {
            get => _isOn;
            set
            {
                if (isOn == value) return;
                _isOn = value;
                TriggerEvents(isOn);
                
                if(isSync)
                    photonView.RPC("SyncState", RpcTarget.Others, isOn, true);
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
        
        public void Switch()
        {
            isOn = !isOn;
        }

        private void OnValidate()
        {
            isOn = On;
        }
    }
}