using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events
{
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Vector 3 Array Event",
        fileName = "New Vector 3 Array Event"
    )]
    public class Vector3ArrayEvent : BaseGameEvent<Vector3[]> { }

    [System.Serializable] public class UnityVector3ArrayEvent : UnityEvent<Vector3[]> { }
}