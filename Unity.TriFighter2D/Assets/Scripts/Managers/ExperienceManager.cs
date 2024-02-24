namespace BlackRece.TriFighter2D.Experience {

    using System;

    using UnityEngine;
    using Events;

    [RequireComponent(typeof(Collider2D))]
    public class ExperienceManager : MonoBehaviour {
        [SerializeField] private float m_baseExperience = 100f;
        [SerializeField] private float m_experienceMultiplier = 1.5f;
        
        private int m_currentLevel = 1;
        private float m_currentExperience = 0f;
        public float CurrentXP {
            get => m_currentExperience;
            set {
                m_currentExperience = value; 
                if(m_currentExperience >= NextLevelExperience)
                    m_currentLevel++;
                EventManager.InvokeEvent(EventIDs.OnUpdateExperienceBar, XPRatio);
            }
        }
        
        public float XPRatio => m_currentExperience / NextLevelExperience;

        public float NextLevelExperience => Mathf.FloorToInt((m_baseExperience * m_currentLevel) * m_experienceMultiplier);
        
        public void GainXP(float a_amount) =>
            CurrentXP += Math.Abs(a_amount);

        public void LoseXP(float a_amount) => 
            CurrentXP -= Math.Abs(a_amount);
    }
}