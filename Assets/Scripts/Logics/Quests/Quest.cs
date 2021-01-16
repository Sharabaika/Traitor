using System;
using System.Collections.Generic;
using MapObjects;
using UnityEngine;

namespace Logics
{
    [Serializable]public class Quest
    {
        [SerializeField] private List<QuestObject> objects;

        [SerializeField] private bool _isCompleted = false;
        
        public string title;
        public string description;

        public List<QuestObject>Objects => objects;

        private List<QuestObject> _completedObjects= new List<QuestObject>();

        public bool UpdateStage(QuestObject completed)
        {
            // TODO хуйня
            if (objects.Contains(completed) && !_completedObjects.Contains(completed))
            {
                _completedObjects.Add(completed);
                if (_completedObjects.Count == objects.Count)
                    _isCompleted = true;
                
                return true;
            }
            return false;
        }
    }
}