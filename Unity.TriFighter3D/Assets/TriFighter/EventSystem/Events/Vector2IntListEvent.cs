using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Vector2Int List Event",
        fileName = "New Vector2Int List Event"
    )]
    public class Vector2IntListEvent : BaseGameEvent<List<Vector2Int>> { }

    [System.Serializable] public class UnityVector2IntListEvent :
        UnityEvent<List<Vector2Int>> { }
}