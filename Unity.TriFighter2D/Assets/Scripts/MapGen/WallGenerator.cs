namespace BlackRece.TriFighter2D.MapGen {
    using System;
    using System.Linq; 
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.Tilemaps;
    
    using RNG = UnityEngine.Random;

    public class WallGenerator : MonoBehaviour {
        public enum TileType {
            None = -1,
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
        [SerializeField] private Vector2Int m_chunkSize = new Vector2Int(10, 10);
        private List<Chunk> m_backChunks = new List<Chunk>();
        private List<Chunk> m_foreChunks = new List<Chunk>();

        #region Unity Functions

        private void Awake() {
            var l_chunkSize = m_mapSize;
            l_chunkSize.x *= m_chunkMultiplier;
            m_chunkSize = l_chunkSize;
        }

        private void Start() {
            // create chunk buffers
            for(int i = 0; i < m_chunkMultiplier; i++) {
                m_backChunks.Add(new Chunk(m_mapSize));
                m_foreChunks.Add(new Chunk(m_mapSize));
            }
            
            // populate chunk buffers
            foreach(Chunk l_chunk in m_backChunks) 
                l_chunk.GenerateRandom();
            foreach(Chunk l_chunk in m_foreChunks)
                l_chunk.GenerateOutline();
            
            // Blit chunks buffers to the tilemaps
            PasteChunks(m_background, m_backChunks);
            PasteChunks(m_foreground, m_foreChunks);
        }
        
        #endregion

        #region TilePallet Functions
        private List<TileBase> GetTiles(TileType a_type) =>
            (m_tilePallet
                .Where(l_tile => l_tile.m_type == a_type)
                .Select(l_tile => l_tile.m_tile))
                .ToList();

        private TileBase GetTile(TileType a_type) {
            var l_tiles = GetTiles(a_type);
            return l_tiles.Count > 0
                ? l_tiles[RNG.Range(0, l_tiles.Count)] 
                : null;
        }
        
        #endregion

        #region TileMap Functions

        private void PasteChunk(Tilemap a_tilemap, Chunk a_chunk, Vector3Int a_offset = default) {
            foreach(var l_tile in a_chunk.ChunkData) 
                a_tilemap.SetTile(l_tile.Key + a_offset, GetTile(l_tile.Value));
        }
        
        private void PasteChunks(Tilemap a_tilemap, List<Chunk> a_chunks) {
            a_tilemap.ClearAllTiles();
            
            Vector3Int l_offset = Vector3Int.zero;
            l_offset.x = m_mapSize.x * m_chunkMultiplier;

            for (var index = 0; index < a_chunks.Count; index++) {
                l_offset.x = index * m_mapSize.x;
                PasteChunk(a_tilemap, a_chunks[index], l_offset);
            }
        }

        #endregion
    }
}