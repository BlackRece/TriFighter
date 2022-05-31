using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Transform List Event",
        fileName = "New Transform List Event"
    )]
    public class TransformListEvent : BaseGameEvent<List<Transform>> { }

    [System.Serializable] public class UnityTransformListEvent : UnityEvent<List<Transform>> { }
}