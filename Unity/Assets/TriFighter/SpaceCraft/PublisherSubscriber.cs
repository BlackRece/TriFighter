using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace TriFighter {
    public interface IPublisher {
        void Publish<T>(T publishedEvent) where T : class;
    }

    public interface ISubscriber {
        void RegisterSubscriber<T>(Action<object> listener) where T : class;
        void UnRegisterSubscriber<T>(Action<object> listener) where T : class;
    }

    public class PublisherSubscriber : IPublisher, ISubscriber {
        private readonly Dictionary<Type, List<Action<object>>> _listeners = 
            new Dictionary<Type, List<Action<object>>>();
        
        public void RegisterSubscriber<T>(Action<object> listener) where T : class {
            var target = typeof(T);
            if(!_listeners.ContainsKey(target))
                _listeners.Add(target, new List<Action<object>>());
            
            _listeners[target].Add(listener);
        }

        public void UnRegisterSubscriber<T>(Action<object> listener) where T : class {
            var target = typeof(T);
            if (_listeners.ContainsKey(target))
                _listeners.Remove(target);
        }

        public void Publish<T>(T publishedEvent) where T : class {
            if(!_listeners.ContainsKey(typeof(T)))
                return;

            foreach (var listener in _listeners[typeof(T)]) 
                listener.Invoke(publishedEvent);
        }
    }

    [CreateAssetMenu(menuName = "Game Event")]
    public sealed class GameEvent : ScriptableObject {
        private HashSet<GameEventListener> _listeners = new HashSet<GameEventListener>();

        public void Invoke() {
            foreach (var listener in _listeners) {
                listener.RaiseEvent();
            }
        }

        public void Register(GameEventListener gameEventListener) =>
            _listeners.Add(gameEventListener);

        public void UnRegister(GameEventListener gameEventListener) =>
            _listeners.Remove(gameEventListener);
    }

    public class GameEventListener : MonoBehaviour {
        [SerializeField] protected GameEvent _gameEvent;
        [SerializeField] protected UnityEvent _unityEvent;

        private void Awake() {
            _gameEvent.Register(this);
        }

        private void OnDestroy() {
            _gameEvent.UnRegister((this));
        }

        public virtual void RaiseEvent() => _unityEvent.Invoke();
    }

    public sealed class GameEventListener<T> : GameEventListener {
        [SerializeField] private T value;

        public override void RaiseEvent() {
            _unityEvent.Invoke();
        }
    }
}