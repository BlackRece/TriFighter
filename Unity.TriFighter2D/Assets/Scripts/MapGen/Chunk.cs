namespace BlackRece.TriFighter2D.MapGen {
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using RNG = UnityEngine.Random;

    public class Chunk {
        public WallGenerator.TileType[] Tiles =>
            m_chunkData.Values.ToArray();

        private BoundsInt m_bounds;
        public BoundsInt Bounds => m_bounds;

        public int xRange => m_bounds.size.x / 2;
        public int yRange => m_bounds.size.y / 2;
        
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

        public void SetTile(Vector3Int a_position, WallGenerator.TileType a_tile) {
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
        
        public Vector3Int GenerateRandom(Vector3Int a_startPosition, float a_fillChance = 0.8f) 
        {
            for (int x = -xRange; x < xRange; x++) {
                for (int y = -yRange; y < yRange; y++) {
                    if(RNG.value > a_fillChance)
                        SetTile(new Vector3Int(x, y, 0), WallGenerator.TileType.Fill);
                }
            }
            return a_startPosition;
        }
        
        public Vector3Int GenerateFill(Vector3Int a_startPosition) {
            for (int x = -xRange; x < xRange; x++) {
                for (int y = -yRange; y < yRange; y++) {
                    SetTile(new Vector3Int(x, y, 0), WallGenerator.TileType.Fill);
                }
            }
            return a_startPosition;
        }
        
        public Vector3Int GenerateOutline(Vector3Int a_startPosition) {
            var l_edgeTop = yRange - 2;
            var l_edgeBot = -yRange + 1;
            
            for (int x = -xRange; x < xRange; x++) {
                for (int y = -yRange; y < yRange; y++) {
                    if(y >= l_edgeTop || y <= l_edgeBot) 
                        SetTile(new Vector3Int(x, y, 0), WallGenerator.TileType.Fill);
                }
            }
            return a_startPosition;
        }

        public Vector3Int GenerateTerrain(Vector3Int a_startPosition, int a_minPathWidth = 6) {
            Vector3Int l_position = a_startPosition;
            int l_pathSpan = Mathf.FloorToInt((float)a_minPathWidth / 2);

            var l_edgeTop = yRange - 2;
            var l_edgeBot = -yRange + 1;

            var l_pathTopMax = l_edgeTop - l_pathSpan;
            var l_pathBotMax = l_edgeBot + l_pathSpan;
            
            // path generation
            //for (int x = a_startPosition.x - xRange; x < a_startPosition.x + xRange; x++) {
            for (int x = -xRange; x < xRange; x++) {
                // keep path within bounds
                // top
                if(l_position.y + l_pathSpan >= l_edgeTop)
                    l_position.y = l_pathTopMax;
                
                // bot
                if(l_position.y - l_pathSpan <= l_edgeBot)
                    l_position.y = l_pathBotMax;
                
                // tunnel fill
                for(int y = -yRange; y < yRange; y++) {
                    // path walls
                    var l_pathTop = l_position.y + l_pathSpan;
                    var l_pathBot = l_position.y - l_pathSpan;
                    
                    //if(y > l_pathTopMax || y < l_pathBotMax) path edge
                    //if(y > l_edgeTop || y < l_edgeBot) extreme edge
                    if(y > l_pathTop || y < l_pathBot) 
                        SetTile(new Vector3Int(x, y, 0), WallGenerator.TileType.Fill);
                }
                
                // get next tile position
                if(x % 3 == 0)
                    l_position.y += RNG.Range(-1, 2);
            }

            return l_position;
        }

        public Vector3Int GenerateTunnelEntrance(Vector3Int a_startPosition, int a_minPathWidth = 10, bool a_forward = true) {
            Vector3Int l_position = a_startPosition;
            int l_pathWidth = Mathf.FloorToInt((float)a_minPathWidth / 2);
            
            // get end positions
            var l_front = new Vector3Int(a_startPosition.x - xRange, l_position.y, 0);
            var l_back = new Vector3Int(a_startPosition.x + xRange, l_position.y, 0);
            
            // get tunnel start and end
            Vector3Int l_tunnelStart, l_tunnelEnd;
            if (a_forward) {
                l_tunnelStart = l_front;
                l_tunnelEnd = l_back;
            } else {
                l_tunnelStart = l_back;
                l_tunnelEnd = l_front;
            }
            
            var l_buffer = 5;
            var l_pathOffset = 0; 
            
            var l_edgeTop = yRange - 2;
            var l_edgeBot = -yRange + 1;
            
            // path generation
            for(int x = l_tunnelStart.x; x < l_tunnelEnd.x; x++) {
                // change path offset every number of buffer tile
                if(x % l_buffer == 0)
                    l_pathOffset ++;
                
                // mark tunnel boundary
                var l_tunnelTop = yRange - l_pathOffset;
                var l_tunnelBot = -yRange + l_pathOffset;
                
                // mark path boundary
                var l_pathTop = l_position.y + l_pathWidth;
                var l_pathBot = l_position.y - l_pathWidth;
                
                for(int y = -yRange; y < yRange; y++) {
                    var l_tile = new Vector3Int(x, y, 0);
                    
                    // keep path clear
                    if (l_tile.y <= l_pathTop && l_tile.y >= l_position.y)
                        l_tile.y = l_pathTop;
                    if (l_tile.y >= l_pathBot && l_tile.y <= l_position.y)
                        l_tile.y = l_pathBot;
                    
                    // fill tunnel walls
                    if (l_tile.y > l_tunnelTop || l_tile.y < l_tunnelBot)
                        SetTile(l_tile, WallGenerator.TileType.Fill);
                    
                    // add path walls
                    if(l_tile.y == l_tunnelTop)
                        SetTile(l_tile, WallGenerator.TileType.T);
                    if(l_tile.y == l_tunnelBot)
                        SetTile(l_tile, WallGenerator.TileType.T);
                }
            }
            return l_position;
        }
    }
}