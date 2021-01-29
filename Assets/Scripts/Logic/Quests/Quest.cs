using System;
using System.Collections.Generic;
using System.Linq;
using MapObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Logic.Quests
{
    [Serializable]public class Quest
    {
        [SerializeField] private List<QuestObject> objects;
        [SerializeField] public UnityEvent OnComplete;

        [SerializeField] private List<QuestTrigger> triggers;

        public string title;
        public string description;
        
        private bool _isCompleted = false;
        public bool isCompleted
        {
            get => _isCompleted;
            set
            {
                if (value && isCompleted==false)
                {
                    _isCompleted = true;
                    Debug.Log(title+" quest completed");
                    OnComplete?.Invoke();
                }
            }
        }

        private void UpdateState()
        {
            Debug.Log(title+ " quest updated");
            if (triggers.All(trigger => trigger.isTriggered))
            {
                isCompleted = true;
            }
        }

        public void OnStart()
        {
            foreach (var trigger in triggers)
            {
                trigger.OnTrigger.AddListener(UpdateState);
            }
        }
    }
}