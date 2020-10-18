using System;
using ExitGames.Client.Photon;
using Items;
using MultiPlayer;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;

namespace Character
{
    public class Inventory : MonoBehaviour, IPunObservable
    {
        [SerializeField]private Weapon[] items = new Weapon[4];
        [SerializeField]private Transform anchor;

        private PhotonView _photonView;
        
        // Remove
        private PlayerMovement _movement;

        public bool isHidden
        {
            get => _isHidden;
            set
            {
                _isHidden = value;
                if(ActiveItem is null)
                    return;
                ActiveItem.gameObject.SetActive(!value);
            }
        }

        private bool _isHidden;
        private int _index = 0;

        private Weapon ActiveItem => items[_index];

        
        /// <summary>
        /// Equips or unequips active slot if choose the same 
        /// </summary>
        [PunRPC]private void EquipSlot(int index)
        {
            if (index == _index) isHidden = !isHidden;
            ChangeActiveSlot(index);
        }
        

        /// <summary>
        /// hides active and enables chosen
        /// </summary>
        private void ChangeActiveSlot(int index)
        {
            if (_index == index)
            {
                return;
            }
            isHidden = true;
            _index = index;
            isHidden = false;
        }

        [PunRPC] private void ShootActiveWeapon()
        {
            if (_photonView.IsMine)
            {
                ActiveItem.Shoot();
            }
            else
            {
                ActiveItem.PlayEffects();
            }
        }

        private void Start()
        {
            isHidden = false;
        }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _movement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            if (_photonView.IsMine)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    // EquipSlot(0);
                    _photonView.RPC("EquipSlot", RpcTarget.All, 0);
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    // ActiveItem.Shoot();
                    _photonView.RPC("ShootActiveWeapon", RpcTarget.All);
                }
                
                
                // Rotate towards cursor
                var d = _movement.PointOfLook - (Vector2)anchor.position;
                var angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
                var rot = anchor.localEulerAngles;
                rot.z = angle;
                anchor.localEulerAngles = rot;

                anchor.localScale = Mathf.Abs(angle) > 90f ? new Vector3(1,-1,1) : new Vector3(1,1,1);
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