using UnityEngine;

namespace TriFighter.Terrain {
    [CreateAssetMenu(menuName = "TriFighter Objects/Terrain/New Terrain Library")]
    public sealed class TerrainLibrary : ScriptableObject {
        [SerializeField] private GameObject _wall;
        [SerializeField] private GameObject _wallMap;

        public void Init() {
            //IoC.Register<IWallMap>(CreateInstance(typeof(IWallMap)));
            IoC.Register<IWallMap>(CreateInstance(typeof(WallMap)));
            
            if (_wall != null) {
                var iWall = _wall.GetComponent<IWall>();
                if (iWall != null) IoC.Register<IWall>(_wall);
            }

            // if (_wallMap != null) {
            //     var wallMap = _wallMap.GetComponent<IWallMap>();
            //     if(wallMap != null) IoC.Register<IWallMap>(_wallMap);
            // }
        }
    }
}