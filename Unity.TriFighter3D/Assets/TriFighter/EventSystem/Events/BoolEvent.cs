using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events
{
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Bool Event",
        fileName = "New Bool Event"
    )]
    public class BoolEvent : BaseGameEvent<bool> { }

    [System.Serializable] public class UnityBoolEvent : UnityEvent<bool> { }
}