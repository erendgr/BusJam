using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Core.Configurations
{
    [CreateAssetMenu(fileName = "PrefabConfigurationSO", menuName = "Bus Jam/Prefab Configuration SO", order = 2)]
    public class PrefabConfigurationSO : ScriptableObject
    {
        [Header("Passenger Prefabs")] 
        public GameObject PassengerPrefab;
        
        [Header("Obstacle Prefabs")] 
        public GameObject ObstaclePrefab;

        [Header("Bus Prefabs")] 
        public GameObject BusPrefab;
    }
}