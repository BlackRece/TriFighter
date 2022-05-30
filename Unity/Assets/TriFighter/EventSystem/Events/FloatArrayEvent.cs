using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Float Array Event",
        fileName = "New Float Array Event"
    )]
    public class FloatArrayEvent : BaseGameEvent<float[]> { }

    [System.Serializable] public class UnityFloatArrayEvent : UnityEvent<float[]> { }
}