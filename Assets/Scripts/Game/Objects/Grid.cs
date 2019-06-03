using System.Collections.Generic;
using Framework.Signals;
using Game.Data;
using Game.Spawn;
using UnityEngine;

namespace Game.Objects
{
    public class Grid : MonoBehaviour
    {
        private bool _isActive;
        private LinkedList<Row> _activeRows;

        [SerializeField] private Transform _rowsRoot;
        [SerializeField] private Spawner _rowSpawner;
        [SerializeField] private Spawner _popSpawner;
        [SerializeField] private Signal _updateSignal;

        private void Awake()
        {
            _activeRows = new LinkedList<Row>();
            SignalsManager.Register(_updateSignal.Name, UpdateGrid);
        }

        public void Activate()
        {
            _isActive = true;

            var startRowCount = GameConfiguration.Instance.GridSettings.StartRowCount;
            for (var i = 0; i < startRowCount; i++)
            {
                SpawnRow(true);
            }

            SpawnRow(false);
            Move(startRowCount);
        }

        public void Deactivate()
        {
            _isActive = false;
        }

        public void MoveDown()
        {
            SpawnRow(true);
            Move();
        }

        public void MoveUp()
        {
            var topRow = GetTopRow();
            if (topRow != null)
            {
                topRow.Deactivate();
            }
            
            Move(-1);
        }
        
        private void UpdateGrid()
        {
            foreach (var row in _activeRows)
            {
                row.CheckHangingCells();
            }

            var first = _activeRows.First;
            if (first != null && first.Value.IsEmpty())
            {
                var next = first.Next;
                while (next != null && next.Value.IsEmpty())
                {
                    if (next.Previous != null)
                    {
                        next.Previous.Value.Deactivate();
                    }
                    
                    next = next.Next;
                }
            }
            else
            {
                SpawnRow(false);
            }

            if (_activeRows.Count >= GameConfiguration.Instance.GridSettings.RowMaxCount)
            {
                MoveUp();
            }
            else if (_activeRows.Count < GameConfiguration.Instance.GridSettings.RowMinCount)
            {
                MoveDown();
            }
        }

        private void Move(int steps = 1)
        {
            foreach (var row in _activeRows)
            {
                row.UpdateRow(row == GetTopRow());
                row.Move(steps);
            }
        }

        private void SpawnRow(bool isTopRow)
        {
            var row = _rowSpawner.Spawn() as Row;
            if (row != null)
            {
                row.transform.SetParent(_rowsRoot);
                row.Deactivated += OnRowDeactivated;

                if (_activeRows.Count > 0)
                {
                    if (isTopRow)
                    {
                        row.Setup(true, _activeRows.Count, GetTopRow(), _popSpawner);
                        _activeRows.AddLast(row);
                    }
                    else
                    {
                        row.Setup(false, 0, GetBottomRow());
                        _activeRows.AddFirst(row);
                    }
                }
                else
                {
                    row.Setup(true, 0, null, _popSpawner);
                    _activeRows.AddLast(row);
                }
            }
        }

        private void OnRowDeactivated(SpawnableObject spawnableObject)
        {
            var row = spawnableObject as Row;
            if (row != null)
            {
                row.Deactivated -= OnRowDeactivated;
                _activeRows.Remove(row);
            }
        }

        private Row GetTopRow()
        {
            if (_activeRows.Count > 0)
            {
                return _activeRows.Last.Value; 
            }

            return null;
        }

        private Row GetBottomRow()
        {
            if (_activeRows.Count > 0)
            {
                return _activeRows.First.Value; 
            }

            return null;
        }
    }
}