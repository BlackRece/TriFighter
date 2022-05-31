using UnityEngine;
namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Void Event",
        fileName = "New Void Event"
    )]
    public class VoidEvent : BaseGameEvent<Void> {
        public void Raise() => Raise(new Void());
    }
    [System.Serializable] public struct Void { }
}