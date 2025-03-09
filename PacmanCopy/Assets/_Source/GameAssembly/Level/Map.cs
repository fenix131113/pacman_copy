using System.Collections.Generic;
using System.Linq;
using Entities.Data;
using UnityEngine;

namespace Level
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private List<FloorCell> mapCells;
        private EntityPath _currentPath;

        private readonly Dictionary<Vector2Int, FloorCell> _mapCells = new();
        
        public IReadOnlyList<FloorCell> MapCells => mapCells;

        private void Awake() => InitializeCells();

        #region Pathing

        public EntityPath GetPathToCell(FloorCell startCell, FloorCell targetCell, List<FloorCell> exceptCells = null)
        {
            return OneWayFind(startCell, targetCell, exceptCells);
        }

        private EntityPath OneWayFind(FloorCell startCell, FloorCell targetCell, List<FloorCell> exceptCells = null)
        {
            HashSet<FloorCell> exceptCellsSet = new();
            
            if(exceptCells != null)
                exceptCellsSet = exceptCells.ToHashSet();
            
            HashSet<FloorCell> visitedCells = new();
            Queue<FloorCell> queue = new();
            Dictionary<FloorCell, FloorCell> cameFrom = new();

            queue.Enqueue(startCell);
            visitedCells.Add(startCell);
            cameFrom[startCell] = null;

            while (queue.Count > 0)
            {
                var currentCell = queue.Dequeue();

                if (currentCell == targetCell)
                    return ReconstructPath(cameFrom, targetCell);

                foreach (var neighbourCell in currentCell.GetNeighboursList()
                             .Where(neighbourCell => !visitedCells.Contains(neighbourCell) && !exceptCellsSet.Contains(neighbourCell)))
                {
                    queue.Enqueue(neighbourCell);
                    visitedCells.Add(neighbourCell);
                    cameFrom[neighbourCell] = currentCell;
                }
            }

            Debug.LogError("No path found!");
            return null;
        }

        private EntityPath ReconstructPath(Dictionary<FloorCell, FloorCell> cameFrom, FloorCell targetCell)
        {
            List<FloorCell> cellPath = new();
            var current = targetCell;

            while (current)
            {
                cellPath.Add(current);
                current = cameFrom[current];
            }

            cellPath.Reverse();

            List<PathRotationGroup> path = new();

            for (var index = 0; index < cellPath.Count; index++)
            {
                var cell = cellPath[index];
                if (index + 1 < cellPath.Count)
                    path.Add(new PathRotationGroup(cell, cell.GetDirectionBySecondCells(cellPath[index + 1])));
            }

            var currentPath = new EntityPath();
            currentPath.SetTargetPath(path);
            return currentPath; 
        }

        #endregion

        public FloorCell GetCell(Vector2Int position)
        {
            if (_mapCells.TryGetValue(position, out var cell))
                return cell;

            Debug.LogWarning($"Cell with coords: {position.ToString()} not found!");
            return null;
        }

        private void InitializeCells()
        {
            foreach (var cell in mapCells)
                _mapCells.Add(
                    new Vector2Int(Mathf.RoundToInt(cell.transform.localPosition.x),
                        Mathf.RoundToInt(cell.transform.localPosition.y)), cell);
        }
    }
}