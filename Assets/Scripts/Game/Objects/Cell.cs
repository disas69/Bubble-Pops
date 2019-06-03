using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Extensions;
using Framework.Signals;
using Game.Data;
using Game.Utils;
using UnityEngine;

namespace Game.Objects
{
    public class Cell : MonoBehaviour
    {
        private Pop _pop;
        private bool _isTop;
        private int _mergeCellsCount;

        [SerializeField] private float _rectDelayTime;
        [SerializeField] private SpriteRenderer _placeHolder;
        [SerializeField] private Signal _updateSignal;
        [SerializeField] private Signal _audioSignal;

        public bool IsTop => _isTop;
        public bool IsEmpty => _pop == null;
        public int Value => _pop != null ? _pop.Value : 0;
        public Pop Pop => _pop;

        private void Awake()
        {
            ActivatePlaceholder(false);
        }

        public void ActivatePlaceholder(bool isActive, int color = 0)
        {
            if (color > 0)
            {
                _placeHolder.color = GameConfiguration.GetColor(color).WithAlpha(_placeHolder.color.a);
            }

            _placeHolder.gameObject.SetActive(isActive);
        }

        public void UpdateCell(bool isTop)
        {
            _isTop = isTop;
        }

        public void React()
        {
            if (_pop == null)
            {
                return;
            }

            _pop.React();
            SignalsManager.Broadcast(_audioSignal.Name, "slap");
            
            this.WaitForSeconds(_rectDelayTime, () =>
            {
                var neighbors = GridUtils.GetNeighbors(this);
                for (var i = 0; i < neighbors.Count; i++)
                {
                    var cell = neighbors[i];
                    if (cell.Pop != null)
                    {
                        cell.Pop.React();
                    }
                }
            });
        }

        public void Attach(Pop pop)
        {
            _pop = pop;
            pop.transform.SetParent(transform);
            pop.transform.localPosition = Vector3.zero;
        }

        public void Detach(bool deactivatePop = false)
        {
            if (_pop != null)
            {
                if (deactivatePop)
                {
                    _pop.Deactivate();
                }
                else
                {
                    _pop.transform.SetParent(null);
                    _pop.Detach();
                }
            }

            _pop = null;
        }

        public void Merge()
        {
            TryMerge(() => SignalsManager.Broadcast(_updateSignal.Name));
        }

        private void TryMerge(Action callback)
        {
            var cluster = GridUtils.GetCluster(this);
            if (cluster.Count > 1)
            {
                StartCoroutine(ProcessMerge(cluster, callback));
            }
            else
            {
                callback?.Invoke();
            }
        }

        private IEnumerator ProcessMerge(List<Cell> cells, Action callback)
        {
            var targetCell = GetFarthestCellInCluster(cells);
            if (targetCell != null)
            {
                _mergeCellsCount = cells.Count - 1;
                for (var i = 0; i < cells.Count; i++)
                {
                    var cell = cells[i];
                    if (cell != targetCell)
                    {
                        cell.Pop.Merge(targetCell, () =>
                        {
                            _mergeCellsCount--;
                            cell.Detach(true);
                        });
                    }
                }
                
                while (_mergeCellsCount > 0)
                {
                    yield return null;
                }
                
                targetCell.Pop.Enhance(cells.Count);
                targetCell.TryMerge(callback);
            }
        }

        private Cell GetFarthestCellInCluster(List<Cell> cells)
        {
            var maxDistance = 0f;
            Cell farthestCell = null;

            foreach (var cell in cells)
            {
                var distance = Vector3.Distance(cell.transform.position, transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthestCell = cell;
                }
            }

            return farthestCell;
        }
    }
}