using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/String Event",
        fileName = "New String Event"
    )]
    public class StringEvent : BaseGameEvent<string> { }

    [System.Serializable] public class UnityStringEvent : UnityEvent<string> { }
}