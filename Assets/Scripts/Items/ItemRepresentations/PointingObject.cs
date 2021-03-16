using UnityEngine;

namespace Items.ItemRepresentations
{
    public class PointingObject : Item
    {
        [SerializeField] private float swingSmoothing = 15;

        private Quaternion _targetRotation;
        protected override void OnUpdate()
        {
            if (isHidden == false && HasOwner)
            {
                var direction = Owner.PointOfLook - transform.position;
                _targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

                if (OwnerIsLocal)
                {
                    transform.rotation = _targetRotation;
                }
                else
                {
                    // smooth
                    transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation,
                        Time.deltaTime * swingSmoothing);
                }
            }
        }
    }
}