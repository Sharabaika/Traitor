using System;
using Character;
using Photon.Pun;
using UnityEngine;

namespace Characters
{
    public class PlayerCharacter : MonoBehaviourPunCallbacks
    {
        private PlayerMovement _movement;
        private CharacterAnimator _animator;
        private Inventory _inventory;
        private Health _health;

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
                    Debug.Log(obj.gameObject.name);

                    // TODO fix
                    obj?.Interact(this);
                }
            }
        }

        private void Start()
        {
            FindObjectOfType<GameManager>().AddPlayer(this);
        }

        public void Kill()
        {
            photonView.RPC("Die", RpcTarget.All);
        }
        
        [PunRPC]public void Die()
        {
            // TODO inappropriate method name 
            DeathEvent?.Invoke();
            
            IsAlive = false;
            Destroy(gameObject, 1);
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
        
        public bool IsImposter = false;
    }
}