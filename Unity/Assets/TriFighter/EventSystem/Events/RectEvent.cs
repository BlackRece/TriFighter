using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Rect Event",
        fileName = "New Rect Event"
    )]
    public class RectEvent : BaseGameEvent<Rect> { }

    [System.Serializable] public class UnityRectEvent : UnityEvent<Rect> { }
}