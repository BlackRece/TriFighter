using System;

using UnityEngine;

namespace TriFighter.Terrain {
    public interface ITerrainController {
        void Init(Transform parentTransform, Rect spaceBoundary);
    }

    [CreateAssetMenu(menuName = "TriFighter Objects/New Terrain Controller")]
    public sealed class TerrainController : ScriptableObject, ITerrainController {
        private IArea _area;
        private Transform _parentTransform;

        private Rect _boundary;
        
        public void Init(Transform parentTransform, Rect spaceBoundary) {
            _parentTransform = parentTransform;

            _boundary = spaceBoundary;
            
            _area = new Area(
                new AreaDetail {
                    Rect = _boundary,
                    Parent = _parentTransform
                }
            );
        }
        
        public void Update() {
            _area.Update();
        }
    }
}