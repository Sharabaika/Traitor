using Characters;
using MapObjects;
using UnityEngine;

namespace Items.ItemRepresentations
{
    public class Lamp : Item
    {
        [SerializeField] private Light lightSource;

        public override void Use(PlayerCharacter by, InteractableObject target = null)
        {
            lightSource.enabled = !lightSource.enabled;
        }
    }
}