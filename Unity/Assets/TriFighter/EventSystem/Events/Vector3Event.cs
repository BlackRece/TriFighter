using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Vector3 Event",
        fileName = "New Vector3 Event"
    )]
    public class Vector3Event : BaseGameEvent<Vector3> { }

    [System.Serializable] public class UnityVector3Event : UnityEvent<Vector3> { }
}