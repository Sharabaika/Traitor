using Characters;
using MapObjects;
using Photon.Pun;
using UnityEngine;

namespace Items.ItemRepresentations
{
    public class Flashlight : PointingObject
    {
        [SerializeField] private Light light;

        public override void Use(PlayerCharacter by, InteractableObject target = null)
        {
            photonView.RPC("TurnOn", RpcTarget.All, !light.gameObject.activeSelf);
        }

        [PunRPC]
        public void TurnOn(bool on)
        {
            light.gameObject.SetActive(on);
        }
    }
}