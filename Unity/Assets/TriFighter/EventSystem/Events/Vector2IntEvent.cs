using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Vector2Int Event",
        fileName = "New Vector2Int Event"
    )]
    public class Vector2IntEvent : BaseGameEvent<Vector2Int> { }

    [System.Serializable] public class UnityVector2IntEvent : UnityEvent<Vector2Int> { }
}