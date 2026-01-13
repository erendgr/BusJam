using System.Collections.Generic;
using _Game._Dev.Scripts.Runtime.Core.Grid;
using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Core.Pathfinding
{
    public interface IPathfindingService
    {
        List<Vector2Int> FindPath(IGrid grid, Vector2Int startPosition, Vector2Int endPosition);
    }
}