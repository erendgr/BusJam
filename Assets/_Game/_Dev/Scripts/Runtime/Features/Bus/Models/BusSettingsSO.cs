using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Features.Bus.Models
{
    [CreateAssetMenu(fileName = "BusSettingsSO", menuName = "Bus Jam/Settings/Bus Settings SO", order = 1)]
    public class BusSettingsSO : ScriptableObject
    {
        public float MoveDuration = 0.3f;
        public float MoveOffset = 20f;
        public int Capacity = 3;
    }
}