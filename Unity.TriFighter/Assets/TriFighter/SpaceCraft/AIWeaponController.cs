using UnityEngine;

namespace TriFighter {
    [CreateAssetMenu(menuName = "New AI WeaponController")]
    public sealed class AIWeaponController : ScriptableObject, IWeaponController {
        public void ActivateWeapon(bool activationState, BulletData bulletData) {
        }

        public void Update() {
        }
    }
}