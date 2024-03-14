namespace BlackRece.TriFighter2D.Collectables {
    using UnityEngine;
    
    using Movement.EnemyMovement;
    using Health;

    public abstract class CollectableBase : MonoBehaviour {
        protected Sprite m_sprite = null;

        public virtual void ApplyEffect(GameObject a_target) { }

        public virtual void OnCollisionEnter2D(Collision2D other) {
            // ignore collision with enemies. can be handled by collision filters in the inspector
            // meaning this method doesn't need to be abstract
            if (other.gameObject.TryGetComponent(out EnemyMovement l_enemy))
                return;
        }

        public Sprite GetSprite() => m_sprite;
    }
    
    public class HealthPickup : CollectableBase {
        private int m_healthAmount;

        public void Init(Sprite a_sprite, int a_healthAmount) {
            m_healthAmount = a_healthAmount;
            
            // do I need to store the sprite here?
            m_sprite = a_sprite;
            
            if(gameObject.TryGetComponent(out SpriteRenderer l_spriteRenderer))
                l_spriteRenderer.sprite = m_sprite;
        }

        public override void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.TryGetComponent(out HealthManager l_HPManager)) {
                if (m_healthAmount >= 0)
                    l_HPManager.Heal(m_healthAmount);
                else
                    l_HPManager.TakeDamage(m_healthAmount);
            }
        }

    }
}
/*
 New Topic:
Replacing conditionals with subtype polymorphism.
Which would you prefer?
1. Have multiple prefab variants
2. Have one base prefab with multiple behaviours attached.
For example; 
 */