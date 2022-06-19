using System;

using TriFighter.Types;

using UnityEngine;

namespace TriFighter.Terrain {
    public class TerrainManager : MonoBehaviour {
        [SerializeField] private TerrainController _terrainController;
        [SerializeField] private TerrainLibrary _terrainLib;

        [SerializeField] private Transform _topLeft, _topRight, _botLeft, _botRight;

        public static FloatRange VerticleRange { get; private set; }
        
        private void Awake() {
            if (_terrainController == null)
                throw new NullReferenceException("No [Terrain Controller] attached!");
            
            if (_terrainLib == null)
                throw new NullReferenceException("No [Terrain Library] attached!");

            if (_topLeft == null || _topRight == null || _botLeft == null || _botRight == null) {
                throw new NullReferenceException("Not all [PlayArea Transform Markers] attached!");
            }
        }

        private void Start() {
            _terrainLib.Init();

            var position = _topLeft.position;
            var playArea = new Rect(
                position.x,
                position.y,
                Vector3.Distance(position, _topRight.position),
                -Vector3.Distance(position, _botLeft.position)
            );

            DEBUG_AreaMarker(playArea);
            
            _terrainController.Init(transform, playArea);
            VerticleRange = new FloatRange(_botLeft.position.y, position.y);
        }
        
        private void Update() {
            _terrainController.Update();
        }

        private void DEBUG_AreaMarker(Rect area) {
            var markers = new GameObject("Markers");
            
            var tl = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tl.name = "tl";
            tl.transform.position = new Vector3(area.xMin, area.yMin);
            tl.transform.SetParent(markers.transform);
            var tr = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tr.name = "tr";
            tr.transform.position = new Vector3(area.xMax, area.yMin);
            tr.transform.SetParent(markers.transform);
            var bl = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bl.name = "bl";
            bl.transform.position = new Vector3(area.xMin, area.yMax);
            bl.transform.SetParent(markers.transform);
            var br = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            br.name = "br";
            br.transform.position = new Vector3(area.xMax, area.yMax);
            br.transform.SetParent(markers.transform);
        }
    }
}