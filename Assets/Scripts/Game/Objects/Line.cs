using System;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

namespace Game.Objects
{
    public class Line : MonoBehaviour
    {
        private const float CastDistance = 15f;
        private const float MaxAngle = 85f;

        private static RaycastComparer _raycastComparer = new RaycastComparer();

        private Vector3 _origin;
        private ContactFilter2D _contactFilter;
        private RaycastHit2D[] _raycastHitResults;

        [SerializeField] private LineRenderer _lineRenderer;

        private void Awake()
        {
            _raycastHitResults = new RaycastHit2D[25];
            _contactFilter = new ContactFilter2D {useTriggers = true};
            _contactFilter.SetLayerMask(LayerMask.NameToLayer("Cell"));
        }

        public Cell Cast(Vector3 origin, Vector3 direction)
        {
            _origin = origin;
            _lineRenderer.SetPosition(0, origin);

            if (Mathf.Abs(Vector3.Angle(Vector3.up, direction)) < MaxAngle)
            {
                ActivateLine(true);

                var hits = Physics2D.Raycast(origin, direction, _contactFilter, _raycastHitResults, CastDistance);

                RaycastHit2D hit;
                var cell = GetFreeCell(_raycastHitResults, hits, out hit);
                if (cell != null)
                {
                    _lineRenderer.SetPosition(1, hit.point);
                    return cell;
                }

                _lineRenderer.SetPosition(1, origin + direction * CastDistance);
            }
            else
            {
                ActivateLine(false);
            }

            return null;
        }

        public void ActivateLine(bool isActive)
        {
            if (_lineRenderer.enabled != isActive)
            {
                _lineRenderer.enabled = isActive;
            }
        }

        private Cell GetFreeCell(RaycastHit2D[] hitResults, int count, out RaycastHit2D hit)
        {
            Array.Sort<RaycastHit2D>(hitResults, 0, count, _raycastComparer);

            var maxDistance = 0f;
            Cell freeCell = null;
            hit = new RaycastHit2D();

            for (var i = 0; i < count; i++)
            {
                var cell = hitResults[i].transform.GetComponent<Cell>();
                if (cell != null)
                {
                    if (!cell.IsEmpty)
                    {
                        break;
                    }

                    if (GridUtils.GetNeighbors(cell).Count > 0)
                    {
                        var distance = Vector3.Distance(cell.transform.position, _origin);
                        if (distance > maxDistance)
                        {
                            maxDistance = distance;
                            freeCell = cell;
                            hit = hitResults[i];
                        }
                    }
                }
            }

            return freeCell;
        }

        private class RaycastComparer : IComparer<RaycastHit2D>
        {
            public int Compare(RaycastHit2D x, RaycastHit2D y)
            {
                return x.distance.CompareTo(y.distance);
            }
        }
    }
}