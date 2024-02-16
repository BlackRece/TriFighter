using UnityEngine;

namespace TriFighter.Terrain {
    [CreateAssetMenu(menuName = "TriFighter Objects/Terrain/New Terrain Library")]
    public sealed class TerrainLibrary : ScriptableObject {
        [SerializeField] private GameObject _wall;
        [SerializeField] private GameObject _wallMap;

        public void Init() {
            if (_wall != null) {
                var iWall = _wall.GetComponent<IWall>();
                if (iWall != null) IoC.Register<IWall>(_wall);
            }
        }
    }
}