using System;
using System.Linq;
using Character;
using Cinemachine;
using ExitGames.Client.Photon;
using Logics;
using MapObjects;
using Photon.Pun;
using Photon.Realtime;
using ScriptableItems;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.VirtualTexturing;
using WebSocketSharp;

namespace Characters
{
    public class PlayerCharacter : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] private DeadBody deadBodyPrefab;
        [SerializeField] private Ghost ghostPrefab;

        [SerializeField] private float maxInteractionDist = 2f;

        [SerializeField] public UnityEvent DeathEvent;
        
        public PlayerMovement Movement { get; private set; }
        public CharacterAnimator Animator{ get; private set; }
        public PlayerInventory Inventory{ get; private set; }
        public Health Health{ get; private set; }
        public Class Class { get; private set; }
        public GameManager.Roles Role { get; private set; }
        
        public bool IsAlive { get; private set; } = true;
        
        public Vector3 PointOfLook { get; private set; }

        private InteractableObject _activeInteractableObject;
        private InteractableObject _interactableObjectOfInterest;
        private Camera _mainCamera;

        private InteractableObject ObjectOfInterest
        {
            get => _interactableObjectOfInterest;
            set
            {
                if(_interactableObjectOfInterest == value)
                    return;

                if (_interactableObjectOfInterest != null)
                    _interactableObjectOfInterest.StopHighlighting();
                
                _interactableObjectOfInterest = value;
               
               if(value!=null)
                   value.HighlightObject(this);
            }
        }
        
        private InteractableObject ActiveInteractableObject
        {
            get => _activeInteractableObject;
            set
            {
                if(_activeInteractableObject!=null)
                    _activeInteractableObject.onStopInteractingLocal.Invoke(this); 
                
                _activeInteractableObject = value;
                
                if(_activeInteractableObject!=null)
                    _activeInteractableObject.Interact(this);
            }
        }

        private void Awake()
        {
            Movement = GetComponent<PlayerMovement>();
            Animator = GetComponent<CharacterAnimator>();
            Inventory = GetComponent<PlayerInventory>();
            Health = GetComponent<Health>();
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if(photonView.IsMine == false)
                return;
            
            // out of dist
            if (ActiveInteractableObject != null)
            {
                if (Vector3.Distance(ActiveInteractableObject.transform.position, transform.position) >
                    maxInteractionDist)
                {
                    ActiveInteractableObject = null;
                }
            }
            
            // interact and look around
            // TODO add interactableObject layer
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            var mask = LayerMask.GetMask("Default", "Characters", "ItemPickup", "InteractableObject");
            if (Physics.Raycast(ray, out var hit, 100f, mask))
            {
                Debug.DrawLine(ray.origin, hit.point);
                PointOfLook = hit.point;
                ObjectOfInterest = hit.collider.gameObject.GetComponent<InteractableObject>();

                if (Input.GetKeyDown(KeyCode.E) && 
                    Vector3.Distance(transform.position, hit.collider.transform.position) < maxInteractionDist)
                {
                        // TODO show hint
                    if(ObjectOfInterest != null)
                        ActiveInteractableObject = ObjectOfInterest;
                }
            }
            
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Inventory.UseActiveItem(this, _interactableObjectOfInterest);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Inventory.AlternativeUseActiveItem(this, _interactableObjectOfInterest);
            }
        }

        private void Start()
        {
            GameManager.Instance.AddPlayer(this);
            // TODO serialize layers
            
            if (photonView.IsMine)
            {
                var virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
                virtualCamera.Follow = transform;

                gameObject.layer = LayerMask.NameToLayer("CurrentPlayer");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Characters");;
            }
        }

        public void Kill()
        {
            photonView.RPC("Die", RpcTarget.All);
        }

        public void Freeze()
        {
            Movement.IsFrozen = true;
        }

        public void Unfreeze()
        {
            Movement.IsFrozen = false;
        }
        
        [PunRPC]public void Die()
        {
            // TODO inappropriate method name 
            DeathEvent?.Invoke();
            
            IsAlive = false;
            Instantiate(deadBodyPrefab, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
            if(photonView.IsMine)
                Instantiate(ghostPrefab, transform.position, Quaternion.identity);
        }
        

        [PunRPC] public void RelocateTo(Vector3 position)
        {
            transform.position = position;
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (targetPlayer == photonView.Owner)
            {
                // check roles
                if (changedProps.TryGetValue("Role", out var str))
                {
                    if (Enum.TryParse((string)str, out GameManager.Roles res))
                    {
                        Role = res;
                        Debug.Log(photonView.Owner.NickName+ " signed role of " + Role.ToString());
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Handles.DrawWireDisc(transform.position,Vector3.up, maxInteractionDist);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(PointOfLook);    
            }
            else
            {
                var point = stream.ReceiveNext();
                PointOfLook = (Vector3) point;
            }
            
        }
    }
}