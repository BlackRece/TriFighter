namespace BlackRece.TriFighter2D.Health {

    using System;

    using UnityEngine;
    using Events;

    public class HealthManager : MonoBehaviour {
        public enum EntityTypes {
            None = 0,
            Player = 1,
            Minion = 2,
            Boss = 3
        }
        
        [SerializeField] private float m_maxHealth = 100f;
        [SerializeField] private float m_currentHealth;

        [SerializeField] private EntityTypes m_entityType = EntityTypes.None;
        public EntityTypes EntityType => m_entityType;
        
        public float CurrentHealth {
            get => m_currentHealth;
            set {
                m_currentHealth = Mathf.Clamp(value, 0f, m_maxHealth);
                
                if(m_entityType == EntityTypes.Player && EventManager.HasEvent(EventIDs.OnUpdateHealthBar))
                    EventManager.InvokeEvent(EventIDs.OnUpdateHealthBar, HealthRatio);

                if (m_currentHealth <= 0f) {
                    string l_eventID = EntityType switch {
                        EntityTypes.Minion => EventIDs.OnMinionDeath,
                        EntityTypes.Boss => EventIDs.OnBossDeath,
                        _ => string.Empty   //EventIDs.OnPlayerDeath
                    };
                    
                    if (EventManager.HasEvent(l_eventID))
                        EventManager.InvokeEvent(l_eventID, gameObject);
                }
            }
        }

        private float HealthRatio => m_currentHealth / m_maxHealth;
        
        private void Awake() {
            Init(EntityType);
        }

        public void Init(EntityTypes a_entityType) {
            m_entityType = a_entityType;
            
            CurrentHealth = m_maxHealth;
        }
        
        public void TakeDamage(float a_damage) =>
            CurrentHealth -= Math.Abs(a_damage);

        public void Heal(float a_heal) => 
            CurrentHealth += Math.Abs(a_heal);
    }
}