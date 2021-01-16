using Character;
using Cinemachine;
using Logics;
using MapObjects;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class PlayerCharacter : MonoBehaviourPunCallbacks
    {
        [SerializeField] private DeadBody deadBodyPrefab;
        [SerializeField] private Ghost ghostPrefab;

        [SerializeField] private int otherPlayerLayer;
        [SerializeField] private int currentPlayerLayer;
        
        private PlayerMovement _movement;
        private CharacterAnimator _animator;
        private Inventory _inventory;
        private Health _health;

        public QuestJournal questJournal { get; set; }
        
        // TODO make private
        public bool IsImposter = false;

        public bool IsAlive { get; private set; } = true;
        
        public UnityEvent DeathEvent;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _animator = GetComponent<CharacterAnimator>();
            _inventory = GetComponent<Inventory>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            // interacting
            if (Input.GetKeyDown(KeyCode.Mouse0) && _inventory.isHidden)
            {
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit)
                {
                    // TODO add user interface
                    
                    var obj = hit.collider.gameObject.GetComponent<InteractableObject>();
                    // TODO show hint
                    obj?.Interact(this);
                }
            }
        }

        private void Start()
        {
            var manager = FindObjectOfType<GameManager>();
            manager.AddPlayer(this);

            if (photonView.IsMine)
            {
                var virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
                virtualCamera.Follow = transform;

                gameObject.layer = currentPlayerLayer;
            }
            else
            {
                gameObject.layer = otherPlayerLayer;
            }
        }

        public void Kill()
        {
            photonView.RPC("Die", RpcTarget.All);
        }

        public void Freeze()
        {
            _movement.IsFrozen = true;
        }

        public void Unfreeze()
        {
            _movement.IsFrozen = false;
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
        

        [PunRPC] public void SignRoles(int prof, bool isImposter)
        {
            IsImposter = isImposter;
        }

        [PunRPC] public void RelocateTo(Vector3 position)
        {
            _movement.Velocity = Vector3.zero;
            transform.position = position;
        }
    }
}