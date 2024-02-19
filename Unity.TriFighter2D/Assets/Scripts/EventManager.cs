namespace BlackRece.TriFighter2D.Events {
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    /*
     * Reminder:
     * In Awake(), call AddEvent<T>(string EventID) for each event you want to use.
     * In OnEnable(), call AddListener<T>(string EventID, Action<T> Action) for each event you want to listen to.
     * In OnDisable(), call RemoveListener<T>(string EventID, Action<T> Action) for each event you want to stop listening to.
     * When you want to broadcast/invoke an event, call InvokeEvent<T>(string EventID, T Data).
    */
    
    // collection of string constants for event names
    public static class EventIDs {
        public const string OnTileEnter = "OnTileEnter";
        public const string OnTileClick = "OnTileClick";

        public const string OnGridReady = "OnGridReady";
        public const string OnGridPositionCheck = "OnGridPositionCheck";
        public const string OnGridStartPosition = "OnGridStartPosition";

        public const string OnUIMoveDirectionClick = "OnDirection";
        public const string OnUICommitClick = "OnUICommitClick";
        
        public const string OnSetTarget = "OnSetTarget";
        public const string OnSetDirection = "OnSetDirection";
        public const string OnPanelStateChange = "OnPanelStateChange";
        public const string OnUICommitMoveClick = "OnUICommitPathClick";
        public const string OnUIShootDirectionClick = "OnUIShootDirectionClick";
        public const string OnUIShootRangeChangeEvent = "OnUIShootRangeChangeEvent";
        
        // UI
        public const string OnUpdateHealthBar = "OnUpdateHealthBar";
        public const string OnUpdateExperienceBar = "OnUpdateExperienceBar";
        public const string OnUpdateShieldBar = "OnUpdateShieldBar";
        
        public const string OnDelAmmo = "OnDelAmmo";
        public const string OnAddAmmo = "OnAddAmmo";
    }
    
    public class EventManager : ScriptableObject {

        private struct EventData {
            public string m_eventName;
            public Type m_eventType;
            private readonly List<Delegate> m_listeners;

            private EventData(string a_eventName, Type a_eventType, List<Delegate> a_listeners) {
                m_eventName = a_eventName;
                m_eventType = a_eventType;
                m_listeners = a_listeners;
            }

            public EventData(string a_eventName, Type a_eventType) =>
                this = new EventData(a_eventName, a_eventType, new List<Delegate>());

            public void AddListener<T>(Delegate a_listener) =>
                m_listeners.Add(a_listener);

            public void RemoveListener(Delegate a_listener) =>
                m_listeners.Remove(a_listener);

            public void InvokeEvent<T>(T a_data) {
                foreach (Delegate l_listener in m_listeners) {
                    if (l_listener is Action<T> action)
                        action?.Invoke(a_data);
                }
            }
        }

        // Dictionary to hold listeners for each type of event
        private static readonly Dictionary<string, EventData> m_eventData = new ();

        public static void AddEvent<T>(string eventName) {
            if (!m_eventData.ContainsKey(eventName)) 
                m_eventData[eventName] = new EventData(eventName, typeof(T));
        }
        
        public static void AddListener<T>(string a_eventName, Action<T> a_listener) {
            if (m_eventData.TryGetValue(a_eventName, out EventData l_eventData))
                l_eventData.AddListener<T>(a_listener);
            else
                throw new KeyNotFoundException($"{a_eventName} not found.");
        }
        
        public static void RemoveListener<T>(string a_eventName, Action<T> a_listener) {
            if (m_eventData.TryGetValue(a_eventName, out EventData l_eventData))
                l_eventData.RemoveListener(a_listener);
            // else
            //     throw new KeyNotFoundException($"{a_eventName} not found.");
        }
        
        public static void InvokeEvent<T>(string a_eventName, T a_data) {
            if (m_eventData.TryGetValue(a_eventName, out EventData l_eventData))
                l_eventData.InvokeEvent(a_data);
            else
                throw new KeyNotFoundException($"{a_eventName} not found.");
        }
        
        public static bool HasEvent(string a_eventName) => 
            m_eventData.ContainsKey(a_eventName);
        
        // cleanup
        public static void RemoveEvent(string a_eventName) {
            if (m_eventData.ContainsKey(a_eventName)) 
                m_eventData.Remove(a_eventName);
        }
        
        public static void RemoveAllEvents() => m_eventData.Clear();

        private void OnDestroy() => RemoveAllEvents();
    }
}