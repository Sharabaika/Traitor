using Character;
using Photon.Pun;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(PhotonView), typeof(CharacterAnimator))]
    public class PlayerMovement : MonoBehaviour, IPunObservable
    {
        [SerializeField] private float speed = 1.5f;
        [SerializeField] private Transform anchor;
        
        private Camera _camera;
        private PhotonView _photonView;
        private CharacterAnimator _animator;

        private Vector3 _lookDirection;

        public Vector3 Velocity { get; set; }
        private float _lastMovementTime = 0f;

        public Vector2 PointOfLook { get; private set; }
        public bool IsFrozen { get; set; }
        
        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            _animator = GetComponent<CharacterAnimator>();
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_photonView.IsMine)
            {
                // Movement
                var dir = new Vector3(
                    Input.GetAxisRaw("Horizontal"),
                    Input.GetAxisRaw("Vertical"),
                    0f).normalized;

                Velocity = dir * speed;

                PointOfLook = _camera.ScreenToWorldPoint(Input.mousePosition);
            }

            Move(Velocity);
            LookAtCursor();
        }

        private void LookAtCursor()
        {
            _animator.IsLookingRight = (PointOfLook.x - anchor.position.x)> 0;
        }

        private void Move(Vector3 velocity)
        {
            if(IsFrozen) velocity = Vector3.zero;

            _animator.Movement(velocity);
            
            transform.Translate(velocity * (Time.time - _lastMovementTime), Space.World);
            _lastMovementTime = Time.time;
        }


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(Velocity);
                stream.SendNext(PointOfLook);
            }
            else
            {
                transform.position = (Vector3)stream.ReceiveNext();
                Velocity = (Vector3) stream.ReceiveNext();
                PointOfLook = (Vector2) stream.ReceiveNext();

                // ???
                // Move(_velocity);
            }
        }
    }
}