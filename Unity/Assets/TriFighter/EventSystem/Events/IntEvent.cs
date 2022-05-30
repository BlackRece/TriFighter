using UnityEngine;
namespace BlackRece.Events {
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Int Event",
        fileName = "New Int Event"
    )]
    public class IntEvent : BaseGameEvent<int> { }
}