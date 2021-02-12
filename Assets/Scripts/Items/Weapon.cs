using Characters;
using Misc;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class Weapon : Item
    {
        public override void HandlePositioning(PlayerCharacter owner)
        {
            transform.LookAt(owner.PointOfLook);
        }
    }
}