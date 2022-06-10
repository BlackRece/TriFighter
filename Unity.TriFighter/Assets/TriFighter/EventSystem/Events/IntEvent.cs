using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Int Event",
        fileName = "New Int Event"
    )]
    public class IntEvent : BaseGameEvent<int> { }
    [System.Serializable] public sealed class UnityIntEvent : UnityEvent<int> { }
}