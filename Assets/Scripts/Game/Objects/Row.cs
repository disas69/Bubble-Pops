using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Main;
using Game.Spawn;
using Game.Utils;
using UnityEngine;

namespace Game.Objects
{
    public enum RowSide
    {
        Left,
        Right
    }

    public class Row : SpawnableObject
    {
        private RowSide _side;
        private float _elapsedTime;
        private Vector3 _currentPosition;

        [SerializeField] private List<Cell> _cells;

        public RowSide Side => _side;

        public void Setup(bool isTopRow, int index, Row neighbor = null, Spawner popSpawner = null)
        {
            _side = GetNextSide(neighbor);
            transform.SetSiblingIndex(index);
            transform.localPosition = GetNextLocalPosition(isTopRow, neighbor, _side);

            if (isTopRow && popSpawner != null)
            {
                for (var i = 0; i < _cells.Count; i++)
                {
                    var cell = _cells[i];
                    var pop = popSpawner.Spawn() as Pop;
                    if (pop != null)
                    {
                        cell.Attach(pop);
                        pop.Setup(GameConfiguration.GetRandomColor(GameController.Instance.GameSession.Level));
                    }
                }
            }
        }

        public override void Deactivate()
        {
            for (var i = 0; i < _cells.Count; i++)
            {
                var cell = _cells[i];
                cell.UpdateCell(false);
                cell.Detach(true);
            }

            base.Deactivate();
        }

        public bool IsEmpty()
        {
            for (var i = 0; i < _cells.Count; i++)
            {
                if (!_cells[i].IsEmpty)
                {
                    return false;
                }
            }

            return true;
        }

        public void Move(int steps)
        {
            _elapsedTime = 0;
            _currentPosition = transform.localPosition;
            StartCoroutine(MoveRoutine(steps));
        }

        public void UpdateRow(bool isTopRow)
        {
            for (var i = 0; i < _cells.Count; i++)
            {
                _cells[i].UpdateCell(isTopRow);
            }
        }

        public void CheckHangingCells()
        {
            for (var i = 0; i < _cells.Count; i++)
            {
                var cell = _cells[i];
                if (cell.IsEmpty || cell.IsTop)
                {
                    continue;
                }

                if (GridUtils.IsHanging(cell))
                {
                    cell.Detach();
                }
            }
        }

        private IEnumerator MoveRoutine(int steps)
        {
            var targetPosition = _currentPosition;
            targetPosition.y -= steps * GameConfiguration.Instance.RowSettings.MoveStep;

            var moveTime = GameConfiguration.Instance.RowSettings.MoveTime;
            while (_elapsedTime < moveTime)
            {
                transform.localPosition = Vector3.Lerp(_currentPosition, targetPosition,
                    GameConfiguration.Instance.RowSettings.MoveCurve.Evaluate(_elapsedTime / moveTime));
                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = targetPosition;
        }

        private RowSide GetNextSide(Row neighbor)
        {
            if (neighbor == null)
            {
                return Random.value < 0.5f ? RowSide.Left : RowSide.Right;
            }

            return neighbor.Side == RowSide.Left ? RowSide.Right : RowSide.Left;
        }

        private Vector2 GetNextLocalPosition(bool isTopRow, Row neighbor, RowSide side)
        {
            var position = Vector2.zero;
            var height = GameConfiguration.Instance.RowSettings.RowHeight;
            
            if (isTopRow)
            {
                if (neighbor != null)
                {
                    position = neighbor.transform.localPosition;
                    position.y += height;
                }
            }
            else
            {
                if (neighbor != null)
                {
                    position = neighbor.transform.localPosition;
                    position.y -= height;
                }
            }

            var shift = GameConfiguration.Instance.RowSettings.RowShift;
            if (side == RowSide.Left)
            {
                position.x = -shift;
            }
            else
            {
                position.x = shift;
            }

            return position;
        }
    }
}