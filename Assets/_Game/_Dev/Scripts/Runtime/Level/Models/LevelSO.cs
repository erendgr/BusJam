using System.Collections.Generic;
using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Level.Models
{
    public enum Colors
    {
        Red,
        Green,
        Blue,
        Yellow,
        Purple
    }

    [System.Serializable]
    public struct PassengerPlacementData
    {
        public Colors Color;
        public Vector2Int GridPosition;
    }

    [System.Serializable]
    public struct ObstaclePlacementData
    {
        public Vector2Int GridPosition;
    }

    [CreateAssetMenu(fileName = "LevelSO", menuName = "Bus Jam/Create LevelSO", order = 0)]
    public class LevelSO : ScriptableObject
    {
        [Header("Level Info")]
        public int LevelIndex = 1;
        
        [Header("Grid Settings")]
        public Vector2Int MainGridSize = new Vector2Int(5, 5);

        [Header("Passenger Placements")]
        public List<PassengerPlacementData> Passengers;

        [Header("Obstacle Placements")]
        public List<ObstaclePlacementData> Obstacles;

        [Header("Bus Settings")]
        public List<Colors> BusColorSequence;
        public Vector3 BusStopPosition = new Vector3(2f, 0.5f, 8f);
        public Vector3 NextBusOffset = new Vector3(-4f, 0f, 0f);
        
        [Header("Level Constraints")]
        public float TimeLimitInSeconds = 120f;
    }
}