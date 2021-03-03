using ScriptableEvents.Events;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableEvents.Listeners
{
    public class ScriptableEventListener<T, E, UER> : MonoBehaviour,
        IGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
    {
        // TODO add listening to multiple events
        [SerializeField] private E gameEvent = null;
        [SerializeField] private bool isListening = true;
        [SerializeField] private UER unityEventResponse = null;

        public E GameEvent {
            get => gameEvent;
            set => gameEvent = value;
        }

        private void OnEnable()
        {
            if (gameEvent == null) { return; }

            GameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (gameEvent == null) return;

            GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T item)
        {
            if(isListening == false)
                return;
            if (unityEventResponse != null)
            {
                unityEventResponse.Invoke(item);
            }
        }
    }
}