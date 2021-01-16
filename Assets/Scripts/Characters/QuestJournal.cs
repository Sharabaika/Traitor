using System;
using System.Collections.Generic;
using MapObjects;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Logics
{
    [Serializable] public class QuestJournal
    {
        [SerializeField] public string name;
        [SerializeField] private List<Quest> _quests;
        
        [SerializeField] private UnityEvent<List<Quest>> OnQuestsUpdate;

        [HideInInspector]public Player owner;
        public void AcceptQuest(Quest quest)
        {
            if(_quests.Contains(quest))
                return;
            _quests.Add(quest);
            OnQuestsUpdate?.Invoke(_quests);
        }

        public void UpdateQuests(QuestObject completed)
        {
            var needToUpdateQuestInterface = false;
            foreach (var quest in _quests)
            {
                if (quest.UpdateStage(completed))
                {
                    needToUpdateQuestInterface = true;
                }
            }
            if(needToUpdateQuestInterface)
                OnQuestsUpdate?.Invoke(_quests);
        }
    }
}