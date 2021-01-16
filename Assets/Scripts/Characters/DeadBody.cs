using Logics;
using MapObjects;
using UnityEngine;

namespace Characters
{
    public class DeadBody : InteractableObject
    {
        public override void Interact(PlayerCharacter with)
        {
            base.Interact(with);
            FindObjectOfType<Voting>().InitiateVoting();
        }
    }
}