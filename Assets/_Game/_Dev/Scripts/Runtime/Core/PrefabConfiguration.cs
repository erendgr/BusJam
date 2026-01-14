using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Core
{
    [CreateAssetMenu(fileName = "PrefabConfigurationSO", menuName = "Bus Jam/Prefab Configuration SO", order = 2)]
    public class PrefabConfigurationSO : ScriptableObject
    {
        [Header("Character Prefabs")] 
        public GameObject PassengerPrefab;
        public GameObject ObstaclePrefab;

        [Header("Bus Prefabs")] public GameObject BusPrefab;
    }
}