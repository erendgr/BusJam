using System.Collections.Generic;
using _Game._Dev.Scripts.Runtime.Core.Grid;
using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Core.Pathfinding
{
    public class PathfindingService : IPathfindingService
    {
        private class PathNode
        {
            public Vector2Int Position;
            public int GCost = int.MaxValue;
            public int HCost;
            public int FCost;
            public PathNode Parent;

            public PathNode(Vector2Int position)
            {
                this.Position = position;
            }

            public void CalculateFCost()
            {
                FCost = GCost + HCost;
            }
        }

        public List<Vector2Int> FindPath(IGrid grid, Vector2Int startPosition, Vector2Int endPosition)
        {
            var openList = new List<PathNode>();
            var closedList = new HashSet<Vector2Int>();
            var pathNodeMap = new Dictionary<Vector2Int, PathNode>();

            var startNode = new PathNode(startPosition)
            {
                GCost = 0,
                HCost = CalculateDistance(startPosition, endPosition)
            };
            startNode.CalculateFCost();
            openList.Add(startNode);
            pathNodeMap[startPosition] = startNode;

            while (openList.Count > 0)
            {
                var currentNode = GetLowestFCostNode(openList);
                if (currentNode.Position == endPosition)
                {
                    return CalculatePath(currentNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode.Position);

                foreach (var neighbourPos in GetNeighbourPositions(grid, currentNode.Position))
                {
                    if (closedList.Contains(neighbourPos)) continue;

                    if (!grid.IsCellAvailable(neighbourPos) && neighbourPos != endPosition)
                    {
                        closedList.Add(neighbourPos);
                        continue;
                    }

                    var tentativeGCost = currentNode.GCost + CalculateDistance(currentNode.Position, neighbourPos);

                    if (!pathNodeMap.TryGetValue(neighbourPos, out var neighbourNode) || tentativeGCost < neighbourNode.GCost)
                    {
                        if (neighbourNode == null)
                        {
                            neighbourNode = new PathNode(neighbourPos);
                            pathNodeMap[neighbourPos] = neighbourNode;
                        }

                        neighbourNode.Parent = currentNode;
                        neighbourNode.GCost = tentativeGCost;
                        neighbourNode.HCost = CalculateDistance(neighbourPos, endPosition);
                        neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }
            return null; 
        }
        
        private List<Vector2Int> CalculatePath(PathNode endNode)
        {
            var path = new List<Vector2Int>();
            path.Add(endNode.Position);
            var currentNode = endNode;
            while (currentNode.Parent != null)
            {
                path.Add(currentNode.Parent.Position);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            return path;
        }

        private int CalculateDistance(Vector2Int a, Vector2Int b)
        {
            var xDistance = Mathf.Abs(a.x - b.x);
            var yDistance = Mathf.Abs(a.y - b.y);
            return 10 * (xDistance + yDistance);
        }
        
        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            var lowestFCostNode = pathNodeList[0];
            for (int i = 1; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].FCost < lowestFCostNode.FCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }
            return lowestFCostNode;
        }

        private List<Vector2Int> GetNeighbourPositions(IGrid grid, Vector2Int currentPosition)
        {
            var neighbourList = new List<Vector2Int>();

            if (currentPosition.x - 1 >= 0) neighbourList.Add(new Vector2Int(currentPosition.x - 1, currentPosition.y));
            if (currentPosition.x + 1 < grid.Width) neighbourList.Add(new Vector2Int(currentPosition.x + 1, currentPosition.y));
            if (currentPosition.y - 1 >= 0) neighbourList.Add(new Vector2Int(currentPosition.x, currentPosition.y - 1));
            if (currentPosition.y + 1 < grid.Height) neighbourList.Add(new Vector2Int(currentPosition.x, currentPosition.y + 1));

            return neighbourList;
        }
    }
}