using _Game._Dev.Scripts.Runtime.Core.Level;
using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Utilities
{
    public static class ColorMapper
    {
        public static Color GetColorFromEnum(Colors color)
        {
            switch (color)
            {
                case Colors.Red: return Color.red;
                case Colors.Green: return Color.green;
                case Colors.Blue: return Color.blue;
                case Colors.Yellow: return Color.yellow;
                case Colors.Purple: return new Color(0.5f, 0, 0.5f);
                default: return Color.white;
            }
        }
    }
}