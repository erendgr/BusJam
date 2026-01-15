using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Features.Passenger.Models
{
    [CreateAssetMenu(fileName = "PassengerSettingsSO", menuName = "Bus Jam/Settings/Passenger Settings SO", order = 1)]
    public class PassengerSettingsSO : ScriptableObject
    {
        public float MoveDuration = 0.2f;
    }
}