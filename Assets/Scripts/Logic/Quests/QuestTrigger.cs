using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Logic.Quests
{
    public class QuestTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent onTrigger;

        public UnityEvent OnTrigger
        {
            get => onTrigger;
        }

        public bool isTriggered { get; private set; } = false;

        public void Trigger()
        {
            if(isTriggered) return;

            isTriggered = true;
            onTrigger?.Invoke();
        }
    }
}