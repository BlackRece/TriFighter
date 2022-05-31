using System.Collections.Generic;
using UnityEngine;
namespace BlackRece.Events {
    public abstract class BaseGameEvent<T> : ScriptableObject {
        private readonly List<IGameEventListener<T>> gameEventListeners = 
            new List<IGameEventListener<T>>();

        public void Raise(T item) {
            for (int i = gameEventListeners.Count - 1; i >= 0; i--) {
                gameEventListeners[i].OnEventRaised(item);
            }
        }

        public void RegisterListener(IGameEventListener<T> listener) {
            if (!gameEventListeners.Contains(listener))
                gameEventListeners.Add(listener);
        }

        public void UnregisterListener(IGameEventListener<T> listener) {
            if (gameEventListeners.Contains(listener))
                gameEventListeners.Remove(listener);
        }
    }
}