using UnityEngine;

namespace TriFighter.Terrain
{
    public interface IWall {
        GameObject GetGameObject { get; }
        Vector3 GameObjectPosition { get; }
        void Hide();
        void MoveBy(float distance);
        void MoveLeft();
        void SetPosition(Vector3 position);
    }
    
    [RequireComponent(typeof(Collider))]
    public class SolidWall : MonoBehaviour, IWall
    {
        private float _speed;
        
        public GameObject GetGameObject => gameObject;
        public Vector3 GameObjectPosition => transform.position;

        public void Hide() {
            gameObject.SetActive(false);
        }

        public void MoveBy(float distance) {
            transform.Translate(Vector3.left * distance * Time.deltaTime);
        }

        public void MoveLeft() {
            gameObject.transform.Translate(Vector3.left * Time.deltaTime);
        }

        public void SetPosition(Vector3 position) {
            transform.position = position;
        }
    }
}
