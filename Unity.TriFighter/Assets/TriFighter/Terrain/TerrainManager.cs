﻿using System;

using UnityEngine;

namespace TriFighter.Terrain {
    public class TerrainManager : MonoBehaviour {
        [SerializeField] private TerrainController _terrainController;
        [SerializeField] private TerrainLibrary _terrainLib;

        private void Awake() {
            if (_terrainController == null)
                throw new NullReferenceException("No [Terrain Controller] attached!");
            
            if (_terrainLib == null)
                throw new NullReferenceException("No [Terrain Library] attached!");
        }

        private void Start() {
            _terrainLib.Init();
            
            _terrainController.Init(
                transform,
                CameraController.GetViewportRect);
        }
        
        private void Update() {
            _terrainController.Update();
        }
    }
}