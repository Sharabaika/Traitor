using System;
using Characters;
using ExitGames.Client.Photon;
using Items;
using MultiPlayer;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;

namespace Character
{
    public class Inventory : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField]private Weapon[] items;
        [SerializeField]private Transform anchor;
        
        // Remove
        private PlayerMovement _movement;
        private PlayerCharacter _character;

        private bool _isHidden;
        private int _activeSlotIndex = 0;
        public Weapon ActiveItem => items[_activeSlotIndex];

        private void EquipSlot(int index)
        {
            if (_activeSlotIndex == index)
            {
                return;
            }

            if(ActiveItem !=null)
                ActiveItem.isHidden = true;
            
            _activeSlotIndex = index;
            
            if(ActiveItem !=null)
                ActiveItem.isHidden = false;

            if (photonView.IsMine)
            {
                var table = new Hashtable {{"activeSlotIndex", index}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(table);
            }
            
            Debug.Log(photonView.Owner + " changed active inventory slot to " + index);
        }

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _character = GetComponent<PlayerCharacter>();
        }

        private void Update()
        {
            if (!photonView.IsMine) return;

            for (var i = 0; i < items.Length; i++)
            {
                if(Input.GetKeyDown((i+1).ToString()))
                    EquipSlot(i);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && ActiveItem!=null)
            {
                ActiveItem.Use(_character);
                // photonView.RPC("UseActiveItem", RpcTarget.All);
            }
                
                
            // Rotate towards cursor
            var d = _movement.PointOfLook - (Vector2)anchor.position;
            var angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
            var rot = anchor.localEulerAngles;
            rot.z = angle;
            anchor.localEulerAngles = rot;

            anchor.localScale = Mathf.Abs(angle) > 90f ? new Vector3(1,-1,1) : new Vector3(1,1,1);
        }

        [PunRPC] private void UseActiveItem()
        {
            if(ActiveItem is null)
                return;
            
            if (photonView.IsMine)
            {
                ActiveItem.Use(_character);
            }
            else
            {
                // ActiveItem.PlayEffects();
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (!photonView.IsMine && targetPlayer == photonView.Owner)
            {
                EquipSlot((int) changedProps["activeSlotIndex"]);
            }
        }


        // Bad
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(anchor.rotation);
                stream.SendNext(anchor.localScale);
                // stream.SendNext(_index);
                // stream.SendNext(isHidden);
            }
            else
            {
                anchor.rotation= (Quaternion) stream.ReceiveNext();
                anchor.localScale = (Vector3) stream.ReceiveNext();
                
                // ChangeActiveSlot((int) stream.ReceiveNext());
                // isHidden = (bool) stream.ReceiveNext();
            }
        }
        
    }
}