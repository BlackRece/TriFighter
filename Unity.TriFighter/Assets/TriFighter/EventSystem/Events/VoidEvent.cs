using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Void Event",
        fileName = "New Void Event"
    )]
    public sealed class VoidEvent : BaseGameEvent<Void> {
        public void Raise() => Raise(new Void());
    }
    [System.Serializable] public sealed class UnityVoidEvent : UnityEvent<Void> { }
    [System.Serializable] public struct Void { }
}