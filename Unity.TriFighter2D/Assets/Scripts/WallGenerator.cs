namespace BlackRece.TriFighter2D.MapGen {
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.Tilemaps;
    
    using RNG = UnityEngine.Random;

    public class WallGenerator : MonoBehaviour {
        public enum TileType {
            Fill = 0,
            TL = 1,
            TR = 2,
            BL = 3,
            BR = 4,
            L = 5,
            R = 6,
            T = 7,
            B = 8,
        }
        
        [Serializable]
        private struct TileMeta {
            public TileType m_type;
            public TileBase m_tile;
        }
        
        // NOTE: Can't directly reference a tile pallet, nor the internal TileMap
        [Header("Tilemaps")]
        [SerializeField] private Tilemap m_background = null;   // non-interactable
        [SerializeField] private Tilemap m_foreground = null;   // interactable

        [Header("Tile Pallet")] 
        [SerializeField] private TileMeta[] m_tilePallet = null;
        
        [Header("Generation Settings")]
        [SerializeField] private Vector2Int m_mapSize = new Vector2Int(40,22);
        
        // Chunking Info
        [SerializeField] private int m_chunkMultiplier = 3;
        [SerializeField] private Vector2Int m_chunkSize;
        private List<Chunk> m_chunks = new List<Chunk>();

        private void Awake() {
            var l_chunkSize = m_mapSize;
            l_chunkSize.x *= m_chunkMultiplier;
        }

        private void Start() {
        }

        #region TilePallet Functions
        private List<TileBase> GetTiles(TileType a_type) => (
            from l_tile in m_tilePallet 
            where l_tile.m_type == a_type
            select l_tile.m_tile
            ).ToList();

        private TileBase GetTile(TileType a_type) {
            var l_tiles = GetTiles(a_type);
            return l_tiles.Count > 0
                ? l_tiles[RNG.Range(0, l_tiles.Count)] 
                : null;
        }
        
        #endregion

        #region TileMap Functions

        private void PasteChunk(Tilemap a_tilemap, Chunk a_chunk) {
            a_tilemap.ClearAllTiles();

            BoundsInt l_bounds = a_chunk.Bounds;
            
            var l_tiles = new List<TileBase>();
            foreach (var l_tile in a_chunk.Tiles)
                l_tiles.AddRange(GetTiles(l_tile));
            
            a_tilemap.SetTilesBlock(a_chunk.Bounds, l_tiles.ToArray());
        }

        #endregion
    }

    internal class Chunk {
        public WallGenerator.TileType[] Tiles =>
            m_chunkData.Values.ToArray();

        private BoundsInt m_bounds;
        public BoundsInt Bounds => m_bounds;

        private Dictionary<Vector3Int, WallGenerator.TileType> m_chunkData = new Dictionary<Vector3Int, WallGenerator.TileType>();
        
        public Chunk(Vector2Int a_size, int a_multiplier) {
            var l_size = new Vector3Int(a_size.x * a_multiplier, a_size.y, 0);
            m_bounds = new BoundsInt(Vector3Int.zero, l_size);
        }
    }
}