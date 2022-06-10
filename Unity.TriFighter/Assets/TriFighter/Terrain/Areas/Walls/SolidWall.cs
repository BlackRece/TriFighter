using System.Collections.Generic;

using UnityEngine;

namespace TriFighter.Terrain
{
    public interface IWall {
        GameObject GetGameObject { get; }
        Vector3 GameObjectPosition { get; }
        void Hide();
    }
    
    public class SolidWall : MonoBehaviour, IWall
    {
        public GameObject GetGameObject => gameObject;
        public Vector3 GameObjectPosition => transform.position;

        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}
