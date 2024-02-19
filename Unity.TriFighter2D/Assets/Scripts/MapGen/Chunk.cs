namespace BlackRece.TriFighter2D.MapGen {
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using RNG = UnityEngine.Random;

    internal class Chunk {
        public WallGenerator.TileType[] Tiles =>
            m_chunkData.Values.ToArray();

        private BoundsInt m_bounds;
        public BoundsInt Bounds => m_bounds;

        private int xRange => m_bounds.size.x / 2;
        private int yRange => m_bounds.size.y / 2;
        
        private int m_index;
        private int m_chunkCount;
        private List<Dictionary<Vector3Int, WallGenerator.TileType>> m_Chunks;
        
        private readonly Dictionary<Vector3Int, WallGenerator.TileType> m_chunkData;
        public Dictionary<Vector3Int, WallGenerator.TileType> ChunkData => m_chunkData;

        public Chunk(Vector2Int a_size) {
            var l_size = new Vector3Int(a_size.x, a_size.y, 0);
            m_bounds = new BoundsInt(Vector3Int.zero, l_size);
            
            m_chunkData = NewChunk();
        }

        private static Dictionary<Vector3Int, WallGenerator.TileType> NewChunk() =>
            new Dictionary<Vector3Int, WallGenerator.TileType>();
        public void Clear() => m_chunkData.Clear();

        private void SetTile(Vector3Int a_position, WallGenerator.TileType a_tile) {
            m_chunkData[a_position] = a_tile;
        }
        
        public WallGenerator.TileType GetTile(Vector3Int a_position) =>
            m_chunkData.ContainsKey(a_position) 
                ? m_chunkData[a_position] 
                : WallGenerator.TileType.None;
        
        public void Generate() {
            for (int x = 0; x < m_bounds.size.x; x++) {
                for (int y = 0; y < m_bounds.size.y; y++) {
                    var l_position = new Vector3Int(x, y, 0);
                    var l_tile = WallGenerator.TileType.Fill;
                    if (x == 0 && y == 0)
                        l_tile = WallGenerator.TileType.TL;
                    else if (x == m_bounds.size.x - 1 && y == 0)
                        l_tile = WallGenerator.TileType.TR;
                    else if (x == 0 && y == m_bounds.size.y - 1)
                        l_tile = WallGenerator.TileType.BL;
                    else if (x == m_bounds.size.x - 1 && y == m_bounds.size.y - 1)
                        l_tile = WallGenerator.TileType.BR;
                    else if (x == 0)
                        l_tile = WallGenerator.TileType.L;
                    else if (x == m_bounds.size.x - 1)
                        l_tile = WallGenerator.TileType.R;
                    else if (y == 0)
                        l_tile = WallGenerator.TileType.T;
                    else if (y == m_bounds.size.y - 1)
                        l_tile = WallGenerator.TileType.B;
                    
                    SetTile(l_position, l_tile);
                }
            }
        }
        
        public void GenerateRandom() {
            for (int x = -xRange; x < xRange; x++) {
                for (int y = -yRange; y < yRange; y++) {
                    var l_position = new Vector3Int(x, y, 0);
                    var l_tile = WallGenerator.TileType.None;
                    l_tile = RNG.value > 0.5f 
                        ? WallGenerator.TileType.Fill 
                        : WallGenerator.TileType.None;
                    
                    SetTile(l_position, l_tile);
                }
            }
        }
        
        public void GenerateFill() {
            for (int x = -xRange; x < xRange; x++) {
                for (int y = -yRange; y < yRange; y++) {
                    var l_position = new Vector3Int(x, y, 0);
                    var l_tile = WallGenerator.TileType.Fill;
                    
                    SetTile(l_position, l_tile);
                }
            }
        }
        
        public void GenerateOutline() {
            for (int x = -xRange; x < xRange; x++) {
                var l_topPosition = new Vector3Int(x, yRange - 2, 0);
                SetTile(l_topPosition, WallGenerator.TileType.T);
                
                var l_botPosition = new Vector3Int(x, -(yRange - 1), 0);
                SetTile(l_botPosition, WallGenerator.TileType.B);
            }
        }

        public void GenerateTerrain() {
            
        }
    }
}