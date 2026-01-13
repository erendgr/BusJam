using System.Collections.Generic;
using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Level.Models
{
    [CreateAssetMenu(fileName = "LevelProgressionSO", menuName = "Bus Jam/Level Progression SO", order = 3)]
    public class LevelProgressionSO : ScriptableObject
    {
        public List<LevelSO> Levels;
    }
}