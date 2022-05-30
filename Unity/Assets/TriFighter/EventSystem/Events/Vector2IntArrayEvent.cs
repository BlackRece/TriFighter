using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Vector2Int Array Event",
        fileName = "New Vector2Int Array Event"
    )]
    public class Vector2IntArrayEvent : BaseGameEvent<Vector2Int[]> { }

    [System.Serializable] public class UnityVector2IntArrayEvent : UnityEvent<Vector2Int[]> { }
}
