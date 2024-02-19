namespace BlackRece.TriFighter2D.UI.HUD {
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;

    using Events;

    public class HUDController : MonoBehaviour {
        [SerializeField] private Image m_healthBarFill;
        [SerializeField] private Image m_experienceBarFill;
        [SerializeField] private Image m_shieldBarFill;
        
        [SerializeField] private GameObject m_modifiersPanel;
        [SerializeField] private GameObject m_modifierPrefab;
        private List<GameObject> m_modifiers;
        
        [SerializeField] private GameObject m_weaponInfoPanel;
        [SerializeField] private GameObject m_ammoSegmentPrefab;
        private List<GameObject> m_ammoSegments;
        private int m_currentAmmo;
        private const int m_maxAmmo = 30;   // Const due to UI design limitation

        private void Awake() {
            m_modifiers = new List<GameObject>();
            m_ammoSegments = new List<GameObject>();
            
            EventManager.AddEvent<float>(EventIDs.OnUpdateHealthBar);
            EventManager.AddEvent<float>(EventIDs.OnUpdateExperienceBar);
            EventManager.AddEvent<float>(EventIDs.OnUpdateShieldBar);
            
            EventManager.AddEvent<int>(EventIDs.OnDelAmmo);
            EventManager.AddEvent<float>(EventIDs.OnAddAmmo);
        }
        
        private void OnEnable() 
        {
            EventManager.AddListener<float>(EventIDs.OnUpdateHealthBar, UpdateHealthBar);
            EventManager.AddListener<float>(EventIDs.OnUpdateExperienceBar, UpdateExperienceBar);
            EventManager.AddListener<float>(EventIDs.OnUpdateShieldBar, UpdateShieldBar);
            
            EventManager.AddListener<int>(EventIDs.OnDelAmmo, DelAmmoSegments);
            EventManager.AddListener<float>(EventIDs.OnAddAmmo, AddAmmoSegments);
        }
        
        private void OnDisable() 
        {
            EventManager.RemoveListener<float>(EventIDs.OnUpdateHealthBar, UpdateHealthBar);
            EventManager.RemoveListener<float>(EventIDs.OnUpdateExperienceBar, UpdateExperienceBar);
            EventManager.RemoveListener<float>(EventIDs.OnUpdateShieldBar, UpdateShieldBar);
            
            EventManager.RemoveListener<int>(EventIDs.OnDelAmmo, DelAmmoSegments);
            EventManager.RemoveListener<float>(EventIDs.OnAddAmmo, AddAmmoSegments);
        }
        
        private void Start() {
            UpdateHealthBar(1f);
            UpdateExperienceBar(0f);
            UpdateShieldBar(0f);
        }

        private void UpdateHealthBar(float a_value) => m_healthBarFill.fillAmount = a_value;
        private void UpdateExperienceBar(float a_value) => m_experienceBarFill.fillAmount = a_value;
        private void UpdateShieldBar(float a_value) => m_shieldBarFill.fillAmount = a_value;
        
        private void AddModifier(string a_modifierName) {
            var l_modifier = Instantiate(m_modifierPrefab, m_modifiersPanel.transform);
            m_modifiers.Add(l_modifier);
        }

        #region Weapon Info - Ammo UI 

        private void AddAmmoSegments(int a_amount) {
            // lets just pray that the amount of segments is not greater than the max ammo
            int l_amount = Mathf.Abs(a_amount);
            int l_diff = m_maxAmmo - m_ammoSegments.Count;
            int l_toAdd = l_amount > l_diff
                ? l_diff
                : l_amount;
            
            for (var i = 0; i < l_toAdd; i++) {
                GameObject l_ammoSegment = Instantiate(m_ammoSegmentPrefab, m_weaponInfoPanel.transform);
                m_ammoSegments.Add(l_ammoSegment);
            }
        }
        
        private void AddAmmoSegments(float a_ratio) {
            int l_amount = Mathf.FloorToInt(m_maxAmmo * a_ratio);
            AddAmmoSegments(l_amount);
        }
        
        private void DelAmmoSegments(int a_amount) {
            if (m_ammoSegments.Count < a_amount)
                return;
            
            for(int i = 0; i < a_amount; i++) {
                int l_index = m_ammoSegments.Count - 1;
                Destroy(m_ammoSegments[l_index]);
                m_ammoSegments.RemoveAt(l_index);
            }
        }
        
        private void DelAmmoSegments(float a_ratio) {
            int l_amount = Mathf.FloorToInt(m_maxAmmo * a_ratio);
            DelAmmoSegments(l_amount);
        }
        
        #endregion

    }
}