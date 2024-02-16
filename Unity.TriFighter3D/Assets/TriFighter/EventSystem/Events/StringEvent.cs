using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/String Event",
        fileName = "New String Event"
    )]
    public sealed class StringEvent : BaseGameEvent<string> { }

    [System.Serializable] public sealed class UnityStringEvent : UnityEvent<string> { }
}