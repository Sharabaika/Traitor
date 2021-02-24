using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Logic.Quests;
using MapObjects;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UserInterface;

namespace Logics.Quests
{
    public class QuestBoard : InteractableObject
    {

        [SerializeField] private List<Quest> quests;
        [SerializeField] private UnityEvent onQuestsComplete;
        
        protected override void OnStart()
        {
            foreach (var quest in quests)
            {
                quest.OnStart();
                quest.OnComplete.AddListener(UpdateQuests);
            }
        }

        private void UpdateQuests()
        {
            if (quests.All(quest => quest.isCompleted))
            {
                Debug.Log("all quests complete");
                onQuestsComplete?.Invoke();
            }
        }
    }
}