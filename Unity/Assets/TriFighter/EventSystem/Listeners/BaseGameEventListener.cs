using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    //This class sits on the GameObject (as a component)
    //and listens to game events of the same type.
    public abstract class BaseGameEventListener<TType, TEvent, TUnityEvent> :
        MonoBehaviour, IGameEventListener<TType> where TEvent :
        BaseGameEvent<TType> where TUnityEvent : UnityEvent<TType> {
        
        [SerializeField] private TEvent gameEvent;
        public TEvent GameEvent {
            get { return gameEvent; }
            set { gameEvent = value; }
        }

        [SerializeField] private TUnityEvent unityEventResponse = null;

        private void OnEnable() {
            if (GameEvent == null) return;

            GameEvent.RegisterListener(this);
        }

        private void OnDisable() {
            if (GameEvent == null) return;

            GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(TType item) {
            if (unityEventResponse != null)
                unityEventResponse.Invoke(item);
        }
    }
}