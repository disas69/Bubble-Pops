using System.Collections.Generic;
using Game.Objects;
using UnityEngine;

namespace Game.Utils
{
    public static class GridUtils
    {
        private const float SearchRadius = 1.1f;
        
        private static Collider2D[] _overlapResults;
        private static ContactFilter2D _contactFilter;

        static GridUtils()
        {
            _overlapResults = new Collider2D[10];
            _contactFilter = new ContactFilter2D {useTriggers = true};
            _contactFilter.SetLayerMask(LayerMask.NameToLayer("Cell"));
        }

        public static List<Cell> GetCluster(Cell cell)
        {
            var result = new List<Cell>();
            var stack = new Stack<Cell>();
            var neighbors = GetNeighbors(cell);
            
            for (var i = 0; i < neighbors.Count; i++)
            {
                var neighbor = neighbors[i];
                if (neighbor.Value == cell.Value)
                {
                    stack.Push(neighbor);
                }
            }

            while (stack.Count > 0)
            {
                var stackCell = stack.Pop();
                result.Add(stackCell);
                neighbors = GetNeighbors(stackCell);
                
                for (var i = 0; i < neighbors.Count; i++)
                {
                    var neighbor = neighbors[i];
                    if (neighbor.Value == stackCell.Value)
                    {
                        if (!stack.Contains(neighbor) && !result.Contains(neighbor))
                        {
                            stack.Push(neighbor);
                        }
                    }
                }
            }

            return result;
        }

        public static bool IsHanging(Cell cell)
        {
            var stack = new Stack<Cell>();
            var visitedCells = new List<Cell>();
            var neighbors = GetNeighbors(cell);
            
            for (var i = 0; i < neighbors.Count; i++)
            {
                var neighbor = neighbors[i];
                if (neighbor.IsTop)
                {
                    return false;
                }

                stack.Push(neighbor);
            }

            while (stack.Count > 0)
            {
                var stackCell = stack.Pop();
                if (stackCell.IsTop)
                {
                    return false;
                }

                visitedCells.Add(stackCell);
                neighbors = GetNeighbors(stackCell);

                for (var i = 0; i < neighbors.Count; i++)
                {
                    var neighbor = neighbors[i];
                    if (!stack.Contains(neighbor) && !visitedCells.Contains(neighbor))
                    {
                        stack.Push(neighbor);
                    }
                }
            }

            return true;
        }

        public static List<Cell> GetNeighbors(Cell cell)
        {
            var neighbors = new List<Cell>();
            var count = Physics2D.OverlapCircle(cell.transform.position, SearchRadius, _contactFilter, _overlapResults);
            for (var i = 0; i < count; i++)
            {
                var neighborCell = _overlapResults[i].gameObject.GetComponent<Cell>();
                if (neighborCell != null && !neighborCell.IsEmpty && neighborCell != cell)
                {
                    neighbors.Add(neighborCell);
                }
            }

            return neighbors;
        }
    }
}