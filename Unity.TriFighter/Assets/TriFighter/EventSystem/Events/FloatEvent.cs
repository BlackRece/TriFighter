using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Float Event",
        fileName = "New Float Event"
    )]
    public sealed class FloatEvent : BaseGameEvent<float> { }
    [System.Serializable] public sealed class UnityFloatEvent : UnityEvent<float> { }
}