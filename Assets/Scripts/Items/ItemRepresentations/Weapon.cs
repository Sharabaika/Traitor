using Characters;
using UnityEngine;

namespace Items
{
    public class Weapon : Item
    {
        [SerializeField] private float swingSmoothing = 15;

        private Quaternion _targetRotation;

        protected override void OnUpdate()
        {
            if (isHidden == false && HasOwner)
            {
                // position + direction = point
                // direction = point - position
                
                var direction = Owner.PointOfLook - transform.position;
                
                _targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

                if (OwnerIsLocal)
                {
                    transform.rotation = _targetRotation;
                }
                else
                {
                    // rough
                    // transform.rotation = 
                    // Quaternion.RotateTowards(transform.rotation, _targetRotation, Time.deltaTime * swingSpeed);

                    // smooth
                    transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation,
                        Time.deltaTime * swingSmoothing);
                }
                
            }
            
        }
    }
}