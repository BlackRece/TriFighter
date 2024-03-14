using System;

namespace BlackRece.TriFighter2D.Collision
{
    using UnityEngine;
    using UnityEngine.Tilemaps;
    
    using Events;
    using Health;
    using Shooting;

    public class CollisionManager : MonoBehaviour
    {
        /*
         * TODO: Handle collision between entities.
         *
         * Player vs.:
         * - Minion
         * - Boss
         * - Projectile
         * - Tilemap
         * - Pickups
         *
         * Pickups vs.:
         * - Player
         * - Enemies: should this be a thing?
         * - Tilemap
         * - Pickups: should not collide with other pickups
         * - Projectile: currently, pickups are destroyed by projectiles
         *
         * Projectile vs.:
         * - Player: currently reduces Player health
         * - Minion: currently reduces Minion health
         * - Boss: will reduce Boss health
         * - Tilemap: currently destroys the projectile
         * - Pickups: currently destroys the projectile. should this be a thing?
         * 
         * Minion vs.:
         * - Player: should deal damage to both the player and the minion
         * - Minion: should do no damage and push each other
         * - Boss: should deal no damage and push Minion
         * - Projectile: should reduce Minion health
         * - Tilemap: minion should move away from the tilemap and be knocked back on collision
         * - Pickups: should not collide with Minions
         *
         * Boss vs.:
         * - Player
         * - Minion
         * - Boss
         * - Projectile
         * - Tilemap
         * - Pickups
         * 3. Minion vs Player
         * 5. Minion vs Minion
         * 7. Minion vs Boss
         * 4. Boss vs Player
         * 6. Boss vs Boss - should never happen unless the boss has multiple parts
         * 
         * 
         */

        private HealthManager m_healthManager;

        private void Start() {
            m_healthManager = GetComponent<HealthManager>();
        }

        private void OnCollisionEnter2D(Collision2D other) {
            // swap between up and down on collision with a tilemap
            if (other.collider is TilemapCollider2D) {
                
            }

            // projectile collision
            if (other.gameObject.TryGetComponent(out ProjectileController l_projectile)) 
                l_projectile.KillProjectile();
            
            // entity collision
            if(other.gameObject.TryGetComponent(out HealthManager l_healthManager)) {
                m_healthManager.TakeDamage(2f);
            
                switch (l_healthManager.EntityType) {
                    case HealthManager.EntityTypes.Player:
                        l_healthManager.TakeDamage(2f);
                        break;
                    case HealthManager.EntityTypes.Minion:
                        break;
                    case HealthManager.EntityTypes.Boss:
                        break;
                }
            }
            
            /* ignore collision with anything else in this list;
             * Boss
             * Pickups
             * 
             */
        }

    }
}
