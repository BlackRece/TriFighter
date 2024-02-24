namespace BlackRece.TriFighter2D.MapGen {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using RNG = UnityEngine.Random;

    public class Chunk {
        public WallGenerator.TileType[] Tiles =>
            m_chunkData.Values.ToArray();

        private BoundsInt m_bounds;
        public BoundsInt Bounds => m_bounds;

        private int xRange => m_bounds.size.x / 2;
        private int yRange => m_bounds.size.y / 2;
        
        private Vector3Int m_startPosition, m_endPosition;
        public Vector3Int StartPosition => m_startPosition;
        public Vector3Int EndPosition => m_endPosition;
        
        private Dictionary<Vector3Int, WallGenerator.TileType> m_chunkData = new();
        public Dictionary<Vector3Int, WallGenerator.TileType> ChunkData => m_chunkData;

        #region Chunk Functons

        public Chunk(Vector2Int a_size) {
            var l_size = new Vector3Int(a_size.x, a_size.y, 0);
            m_bounds = new BoundsInt(Vector3Int.zero, l_size);
        }

        public void CopyChunk(Chunk other) {
            // m_chunkData = new Dictionary<Vector3Int, WallGenerator.TileType>();
            // foreach (var tile in other.m_chunkData) 
            //     m_chunkData.Add(tile.Key, tile.Value);
            
            m_bounds = other.Bounds;
            m_chunkData = new Dictionary<Vector3Int, WallGenerator.TileType>(other.ChunkData);
            m_startPosition = other.StartPosition;
            m_endPosition = other.EndPosition;
        }

        private void SetTile(Vector3Int a_position, WallGenerator.TileType a_tile) => 
            m_chunkData[a_position] = a_tile;

        private void DelTile(Vector3Int a_position) {
            if (m_chunkData.ContainsKey(a_position))
                m_chunkData.Remove(a_position);
        } 

        public WallGenerator.TileType GetTile(Vector3Int a_position) =>
            m_chunkData.ContainsKey(a_position) 
                ? m_chunkData[a_position] 
                : WallGenerator.TileType.None;

        #endregion  // Chunk Functons
        
        #region Generation Functions
        
        public Vector3Int GenerateRandom(Vector3Int a_startPosition, float a_fillChance = 0.8f) 
        {
            var l_position = a_startPosition;
            l_position.x = -xRange;
            m_startPosition = l_position;
            
            for (int x = -xRange; x < xRange; x++) {
                for (int y = -yRange; y < yRange; y++) {
                    if(RNG.value > a_fillChance)
                        SetTile(new Vector3Int(x, y, 0), WallGenerator.TileType.Fill);
                }
            }
            l_position.x = xRange;
            m_endPosition = l_position;
            return m_endPosition;
        }
        
        public Vector3Int GenerateFill(Vector3Int a_startPosition) {
            var l_position = a_startPosition;
            l_position.x = -xRange;
            m_startPosition = l_position;

            for (int x = -xRange; x < xRange; x++) {
                for (int y = -yRange; y < yRange; y++) {
                    SetTile(new Vector3Int(x, y, 0), WallGenerator.TileType.Fill);
                }
            }
            
            l_position.x = xRange;
            m_endPosition = l_position;
            return m_endPosition;
        }
        
        public Vector3Int GenerateOutline(Vector3Int a_startPosition) {
            var l_position = a_startPosition;
            l_position.x = -xRange;
            m_startPosition = l_position;

            var l_edgeTop = yRange - 2;
            var l_edgeBot = -yRange + 1;
            
            for (int x = -xRange; x < xRange; x++) {
                for (int y = -yRange; y < yRange; y++) {
                    if(y >= l_edgeTop || y <= l_edgeBot) 
                        SetTile(new Vector3Int(x, y, 0), WallGenerator.TileType.Fill);
                }
            }
            
            l_position.x = xRange;
            m_endPosition = l_position;
            return m_endPosition;
        }

        public Vector3Int GenerateTerrain(Vector3Int a_startPosition, int a_minPathWidth = 6) {
            int l_yPos = a_startPosition.y;
            m_startPosition = new Vector3Int(-xRange, l_yPos);
            
            int l_pathSpan = Mathf.FloorToInt((float)a_minPathWidth / 2);

            int l_edgeTop = yRange - 3;
            int l_edgeBot = -yRange + 2;

            int l_pathTopMax = l_edgeTop - l_pathSpan;
            int l_pathBotMax = l_edgeBot + l_pathSpan;
            
            // path generation
            for (int x = -xRange; x < xRange; x++) {
                // keep path within bounds
                int l_y = l_yPos;
                    
                // top
                if(l_y + l_pathSpan > l_edgeTop)
                    l_y = l_pathTopMax;
                        
                // bot
                if(l_y - l_pathSpan < l_edgeBot)
                    l_y = l_pathBotMax;
            
                // tunnel fill
                for(int y = -yRange; y < yRange; y++) {
                    // path walls
                    int l_pathTop = l_y + l_pathSpan;
                    int l_pathBot = l_y - l_pathSpan;
                    
                    if(y > l_pathTop || y < l_pathBot) 
                        SetTile(new Vector3Int(x, y, 0), WallGenerator.TileType.Fill);
                    else
                        DelTile(new Vector3Int(x, y, 0));
                }
                
                // get next tile position
                if(x % 3 == 0)
                    l_y  += RNG.Range(-1, 2);
                
                l_yPos = l_y;
            }

            m_endPosition = new Vector3Int(xRange, l_yPos);
            return m_endPosition;
        }

        public Vector3Int GenerateTunnelEntrance(Vector3Int a_startPosition, int a_minPathWidth = 10) {
            m_startPosition = a_startPosition;
            m_startPosition.x = -xRange;
            
            int l_pathWidth = Mathf.FloorToInt((float)a_minPathWidth / 2);
            
            m_endPosition = new Vector3Int(m_startPosition.x + m_bounds.xMax, m_startPosition.y, 0);
            
            var l_buffer = 5;
            var l_pathOffset = 0; 
            
            var l_edgeTop = yRange - 2;
            var l_edgeBot = -yRange + 1;
            
            // mark path boundary
            var l_pathTop = m_startPosition.y + l_pathWidth;
            var l_pathBot = m_startPosition.y - l_pathWidth;
            
            // path generation
            for(int x = m_startPosition.x; x < m_endPosition.x; x++) {
                // change path offset every number of buffer tile
                if(x % l_buffer == 0)
                    l_pathOffset ++;
                
                // mark tunnel boundary
                var l_tunnelTop = l_edgeTop - l_pathOffset;
                var l_tunnelBot = l_edgeBot + l_pathOffset;

                for(int y = -yRange; y < yRange; y++) {
                    var l_tile = new Vector3Int(x, y, 0);
                    
                    // keep path clear
                    if(y < l_pathTop && y > l_pathBot)
                        continue;
                    
                    // fill tunnel walls
                    if(y > l_tunnelTop || y < l_tunnelBot)
                        SetTile(l_tile, WallGenerator.TileType.Fill);
                    
                    // add path walls
                    if(y ==  l_tunnelTop)
                        SetTile(l_tile, WallGenerator.TileType.B);
                    if(y == l_tunnelBot)
                        SetTile(l_tile, WallGenerator.TileType.T);
                }
            }
            
            return m_endPosition;
        }
        
        #endregion  // Generation Functions
    }
}