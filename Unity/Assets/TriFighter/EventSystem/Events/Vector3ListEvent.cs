using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Vector3 List Event",
        fileName = "New Vector3 List Event"
    )]
    public class Vector3ListEvent : BaseGameEvent<List<Vector3>> { }

    [System.Serializable] public class UnityVector3ListEvent : UnityEvent<List<Vector3>> { }
}