using UnityEngine;
using UnityEngine.Events;

namespace BlackRece.Events
{
    [CreateAssetMenu(
        menuName = "BlackRece/Game Events/Game Object Event",
        fileName = "New Game Object Event"
    )]
    public class GameObjectEvent : BaseGameEvent<GameObject> { }

    [System.Serializable] public class UnityGameObjectEvent : UnityEvent<GameObject> { }
}