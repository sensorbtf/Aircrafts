using UnityEngine;

namespace World
{
    public class WorldManager : MonoBehaviour
    {
        public GameObject CellPrefab; 
        public bool IsUnderground; 
        public int gridWidth = 200; 
        public int gridHeight = 200;
        public float cellSize = 1f; 
        public Vector2 undergroundStartPosition = new Vector2(0, -231); 

        void Start()
        {
            GenerateUndergroundWorld();
        }

        private void GenerateUndergroundWorld()
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector2 cellPosition = new Vector2(x * cellSize, y * cellSize) + undergroundStartPosition;
                    Instantiate(CellPrefab, cellPosition, Quaternion.identity, transform);
                }
            }
        }
    }
}