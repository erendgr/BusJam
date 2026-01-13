using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Core.Grid
{
    [CreateAssetMenu(fileName = "GridConfigurationSO", menuName = "Bus Jam/Grid Configuration SO", order = 1)]
    public class GridConfigurationSO : ScriptableObject
    {
        [Header("Tile Prefabs")] 
        public GameObject MainGridTilePrefab;
        public GameObject WaitingAreaTilePrefab;
        
        [Header("Waiting Area Dimensions")]
        public Vector2Int WaitingGridSize = new Vector2Int(5, 1);
        public float SpacingBetweenGrids = 2f;
    }
}