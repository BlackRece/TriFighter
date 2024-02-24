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
        [SerializeField] private Tilemap m_debugMap = null;    // debug

        [Header("Tile Pallet")] 
        [SerializeField] private TileMeta[] m_tilePallet = null;
        [SerializeField] private TileBase m_ruleTile = null;
        
        [Header("Generation Settings")]
        [SerializeField] private Vector2Int m_mapSize = new Vector2Int(40,22);

        [SerializeField, Range(5, 25)] private int m_minPathWidth = 10;
        
        [Header("Scroll Settings")]
        [SerializeField] private float m_backScrollSpeed = .5f;
        [SerializeField] private float m_foreScrollSpeed = 2f;
        [SerializeField] private Vector3 m_origin = Vector3.zero;
        
        private float m_maxScroll => (m_mapSize.x * m_chunkMultiplier) - m_mapSize.x;
        private float m_minScroll => -(m_mapSize.x / 2);
        
        // Chunking Info
        [SerializeField] private int m_chunkMultiplier = 3;
        [SerializeField] private Vector2Int m_chunkSize = new Vector2Int(10, 10);
        private List<Chunk> m_backChunks = new List<Chunk>();
        private List<Chunk> m_foreChunks = new List<Chunk>();
        private Chunk m_debugChunk;
        private Vector3Int m_foreStartPos;
        
        private bool m_isPaused;

        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }

        #region Unity Functions

        private void Awake() {
            var l_chunkSize = m_mapSize;
            l_chunkSize.x *= m_chunkMultiplier;
            m_chunkSize = l_chunkSize;
            
            m_minPathWidth = Mathf.Clamp(m_minPathWidth, 8, m_mapSize.y - 2);
        }

        private void Start() {
            IsPaused = false;
            
            // create chunk buffers
            m_debugChunk = new Chunk(m_mapSize);
            for(int i = 0; i < m_chunkMultiplier; i++) {
                m_backChunks.Add(new Chunk(m_mapSize));
                m_foreChunks.Add(new Chunk(m_mapSize));
            }
            
            // populate chunk buffers
            m_foreStartPos = m_foreChunks[0].GenerateOutline(default);
            m_foreStartPos = m_foreChunks[1].GenerateTunnelEntrance(m_foreStartPos, m_minPathWidth);
            m_foreStartPos = m_foreChunks[2].GenerateTerrain(m_foreStartPos, m_minPathWidth);
            
            Vector3Int l_startPosBack = default;
            foreach (Chunk l_chunk in m_backChunks)
                l_startPosBack = l_chunk.GenerateOutline(l_startPosBack);
            
            // Blit chunks buffers to the tilemaps
            PasteChunks(m_foreground, m_foreChunks);
            PasteChunks(m_background, m_backChunks);
        }

        private void Update() {
            if (m_isPaused)
                return;
            
            m_foreground.transform.Translate(Vector3.left * (m_foreScrollSpeed * Time.deltaTime));
            m_background.transform.Translate(Vector3.left * (m_backScrollSpeed * Time.deltaTime));

            if (m_foreground.transform.position.x <= -m_maxScroll) {
                // debug
                m_debugMap.ClearAllTiles();
                m_debugChunk.CopyChunk(m_foreChunks[2]);
                PasteChunk(m_debugMap, m_debugChunk);
                m_debugMap.RefreshAllTiles();
                
                // shift chunks
                m_foreChunks[0].CopyChunk(m_foreChunks[2]);
                m_foreChunks[1].GenerateTerrain(m_foreChunks[0].EndPosition, m_minPathWidth);
                m_foreChunks[2].GenerateTerrain(m_foreChunks[1].EndPosition, m_minPathWidth);
                
                PasteChunks(m_foreground, m_foreChunks);
                
                // reset position
                m_foreground.transform.position = m_origin;
            }

            if (m_background.transform.position.x <= -m_maxScroll)
                m_background.transform.position = new Vector3(m_origin.x - m_minScroll, m_origin.y);
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
        
        #endregion  // TilePallet Functions

        #region TileMap Functions

        private void PasteChunk(Tilemap a_tilemap, Chunk a_chunk, Vector3Int a_offset = default) {
            foreach(var l_tile in a_chunk.ChunkData) 
                a_tilemap.SetTile(l_tile.Key + a_offset, m_ruleTile);
        }
        
        private void PasteChunks(Tilemap a_tilemap, List<Chunk> a_chunks) {
            a_tilemap.ClearAllTiles();

            Vector3Int l_offset = Vector3Int.zero;
            l_offset.x = m_mapSize.x * m_chunkMultiplier;

            for (var index = 0; index < a_chunks.Count; index++) {
                l_offset.x = index * m_mapSize.x;
                PasteChunk(a_tilemap, a_chunks[index], l_offset);
            }
            
            a_tilemap.RefreshAllTiles();
        }

        #endregion  // TileMap Functions
    }
}