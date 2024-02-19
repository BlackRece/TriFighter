namespace BlackRece.TriFighter2D.Health {

    using System;

    using UnityEngine;
    using Events;

    public class HealthManager : MonoBehaviour {
        public enum EntityTypes {
            None = 0,
            Player = 1,
            Enemy = 2
        }
        
        [SerializeField] private float m_maxHealth = 100f;
        [SerializeField] private float m_currentHealth;

        [SerializeField] private EntityTypes m_entityType = EntityTypes.None;
        public EntityTypes EntityType => m_entityType;
        
        private float CurrentHealth {
            get => m_currentHealth;
            set {
                m_currentHealth = Mathf.Clamp(value, 0f, m_maxHealth);
                if(m_entityType == EntityTypes.Player && EventManager.HasEvent(EventIDs.OnUpdateHealthBar))
                    EventManager.InvokeEvent(EventIDs.OnUpdateHealthBar, HealthRatio);
            }
        }

        private float HealthRatio => m_currentHealth / m_maxHealth;
        
        private void Awake() {
            Init(EntityType);
        }
        
        private void Init(EntityTypes a_entityType) {
            m_entityType = a_entityType;
            
            switch (EntityType) {
                case EntityTypes.Player:
                    //already added in HUDController.Awake()
                    //EventManager.AddEvent<float>(EventIDs.OnUpdateHealthBar);
                    break;
                case EntityTypes.Enemy:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(a_entityType), a_entityType, null);
            }
            
            CurrentHealth = m_maxHealth;
        }
        
        public void TakeDamage(float a_damage) =>
            CurrentHealth -= Math.Abs(a_damage);

        public void Heal(float a_heal) => 
            CurrentHealth += Math.Abs(a_heal);
    }
}