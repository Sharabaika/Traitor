using Characters;
using UnityEngine;

namespace MapObjects.Objects
{
    public class Door : InteractableObject
    {
        private Animator _animator;

        protected override void OnAwake()
        {
            _animator.GetComponent<Animator>();
        }

    }
}