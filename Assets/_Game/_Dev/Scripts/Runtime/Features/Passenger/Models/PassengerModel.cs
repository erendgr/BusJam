using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Features.Passenger.Models
{
    public class PassengerModel
    {
        public bool IsMoving { get; set; }
        public Vector2Int GridPosition { get; private set; }
        
        public PassengerModel(Vector2Int startGridPos)
        {
            GridPosition = startGridPos;
        }
        
        public void UpdateGridPosition(Vector2Int pos) => GridPosition = pos;
    }
}