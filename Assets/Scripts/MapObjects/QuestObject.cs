using Characters;
using Logics;
using UnityEngine;

namespace MapObjects
{
    public class QuestObject : InteractableObject
    {
        public override void Interact(PlayerCharacter player)
        {
            base.Interact(player);
            player.questJournal.UpdateQuests(this);
        }
    }
}