namespace BlackRece.TriFighter2D.Collectables {

    using System;

    using UnityEngine;
    
    using Movement.EnemyMovement;
    using Experience;
    using Health;

    public enum CollectableType {
        None = 0,
        Health = 1,
        Ammo = 2,
        Experience = 3
    }

    public class CollectablePickUp : MonoBehaviour {
        [SerializeField] private CollectableType m_collectableType = CollectableType.None;

        [SerializeField] private int m_rewardAmount = 0;
        
        /*
         * these sprite needs to be moved to either each collectable subtype or to the collectable spawner
         * 
         */
        [SerializeField] private Sprite m_healthSprite = null;
        [SerializeField] private Sprite m_ammoSprite = null;
        [SerializeField] private Sprite m_experienceSprite = null;
        
        private GameObject m_sprite = null;

        private void Awake() {
            if (m_healthSprite == null)
                throw new Exception("Health Sprite not set");
            if (m_ammoSprite == null)
                throw new Exception("Ammo Sprite not set");
            if (m_experienceSprite == null)
                throw new Exception("Experience Sprite not set");
            
            m_sprite = transform.GetChild(0).gameObject;
            if(m_sprite == null)
                throw new Exception("Sprite child object not found");
            
            Init(m_collectableType, m_rewardAmount);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            // ignore collision with enemies. can be handled by collision filters in the inspector
            if (other.gameObject.TryGetComponent(out EnemyMovement l_enemy))
                return;
            
            switch (m_collectableType) {
                case CollectableType.Health:
                    if (other.gameObject.TryGetComponent(out HealthManager l_HPManager)) {
                        if (m_rewardAmount >= 0)
                            l_HPManager.Heal(m_rewardAmount);
                        else
                            l_HPManager.TakeDamage(m_rewardAmount);
                        
                        Destroy(gameObject);
                    }
                        
                    break;
                case CollectableType.Ammo:
                    break;
                case CollectableType.Experience:
                    if (other.gameObject.TryGetComponent(out ExperienceManager l_XPManager)) {
                        if(m_rewardAmount >= 0)
                            l_XPManager.GainXP(m_rewardAmount);
                        else
                            l_XPManager.LoseXP(m_rewardAmount);
                        
                        Destroy(gameObject);
                    }
                    break;
            }
        }

        public void Init(CollectableType a_type, int a_amount) {
            m_collectableType = a_type == CollectableType.None
                ? CollectableType.Health
                : a_type;

            m_rewardAmount = a_amount;
            
            if(m_sprite.TryGetComponent(out SpriteRenderer l_spriteRenderer))
                l_spriteRenderer.sprite = m_collectableType switch {
                    CollectableType.Health => m_healthSprite,
                    CollectableType.Ammo => m_ammoSprite,
                    CollectableType.Experience => m_experienceSprite
                };
            else
                throw new Exception("SpriteRenderer not found on child object");
            
        }
        
        public void Init_V2(CollectableType a_type, int a_amount) {
            // wrong place?
            /*
             * the aim is to use subtype polymorphism to create a new collectable,
             * thus removing conditional statements
             *
             * this is the wrong place to do this, as the collectable type is already set
             * in the inspector
             *
             * ideally, the collectable type would be assigned by instantiating a new collectable
             * and calling the Init method.
             *
             */
            
            // TESTING: get sprite from child object
            m_sprite = transform.GetChild(0).gameObject;
            
            // create new collectable using subtype of base
            var l_healthPickup = new HealthPickup();
            l_healthPickup.Init(m_healthSprite, m_rewardAmount);
            
            
        }

    }
}