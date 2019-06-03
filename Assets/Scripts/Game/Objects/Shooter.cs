using Framework.Extensions;
using Framework.Input;
using Framework.Signals;
using Game.Data;
using Game.Main;
using Game.Spawn;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Objects
{
    public class Shooter : MonoBehaviour
    {
        private bool _isActive;
        private bool _isDragging;
        private Pop _firstPop;
        private Pop _secondPop;
        private Cell _currentCell;
        private Vector3 _aimDirection;

        [SerializeField] private float _activationTime;
        [SerializeField] private float _reloadingTime;
        [SerializeField] private Camera _camera;
        [SerializeField] private Line _line;
        [SerializeField] private Cell _firstPopCell;
        [SerializeField] private Cell _secondPopCell;
        [SerializeField] private Spawner _popSpawner;
        [SerializeField] private Signal _audioSignal;

        private void Awake()
        {
            InputEventProvider.Instance.PointerDown += OnPointerDown;
            InputEventProvider.Instance.Drag += OnDrag;
            InputEventProvider.Instance.PointerUp += OnPointerUp;
        }

        public void Activate()
        {
            _firstPop = GetNewPop(_firstPopCell);
            _secondPop = GetNewPop(_secondPopCell);
            this.WaitForSeconds(_activationTime, () => _isActive = true);
        }

        private void Update()
        {
            if (_isActive && _isDragging)
            {
                var cell = _line.Cast(_firstPopCell.transform.position, _aimDirection);
                if (cell != null)
                {
                    if (cell != _currentCell)
                    {
                        ResetCurrentCell();
                        _currentCell = cell;
                        _currentCell.ActivatePlaceholder(true, _firstPop.Value);
                    }
                }
                else
                {
                    ResetCurrentCell();
                }
            }
        }

        public void Deactivate()
        {
            _isActive = false;
        }
        
        public void SwapPops()
        {
            if (_isActive)
            {
                _isActive = false;
                _firstPop.Move(_secondPopCell, _reloadingTime, () =>
                {
                    _firstPop.transform.SetParent(_secondPopCell.transform);
                });
                
                _secondPop.Move(_firstPopCell, _reloadingTime, () =>
                {
                    _secondPop.transform.SetParent(_firstPopCell.transform);
                    
                    var temp = _firstPop;
                    _firstPop = _secondPop;
                    _secondPop = temp;
                    _isActive = true;
                });
            }
        }

        private Pop GetNewPop(Cell cell)
        {
            var pop = _popSpawner.Spawn() as Pop;
            if (pop != null)
            {
                pop.Setup(GameConfiguration.GetRandomColor(GameController.Instance.GameSession.Level));
                pop.transform.SetParent(cell.transform);
                pop.transform.localPosition = Vector3.zero;
                return pop;
            }

            return null;
        }

        private void OnPointerDown(PointerEventData eventData)
        {
            _isDragging = true;
            _aimDirection = GetAimDirection(eventData);
        }

        private void OnDrag(PointerEventData eventData)
        {
            _aimDirection = GetAimDirection(eventData);
        }

        private void OnPointerUp(PointerEventData eventData)
        {
            _isDragging = false;
            _line.ActivateLine(false);

            if (_isActive && _currentCell != null)
            {
                Shoot(_currentCell);
            }
            
            ResetCurrentCell();
        }

        private void Shoot(Cell cell)
        {
            _isActive = false;
            _firstPop.Move(cell, () =>
            {
                cell.Attach(_firstPop);
                cell.React();
                cell.Merge();
            });
            
            SignalsManager.Broadcast(_audioSignal.Name, "shoot");
            Reload();
        }

        private void Reload()
        {
            _secondPop.Move(_firstPopCell, _reloadingTime, () =>
            {
                _secondPop.transform.SetParent(_firstPopCell.transform);
                _firstPop = _secondPop;
                _secondPop = GetNewPop(_secondPopCell);
                _isActive = true;
            });
        }

        private Vector3 GetAimDirection(PointerEventData eventData)
        {
            var worldPoint = _camera.ScreenToWorldPoint(eventData.position);
            worldPoint.z = 0f;

            var aimDirection = worldPoint - _firstPopCell.transform.position;
            aimDirection.z = 0f;

            return aimDirection.normalized;
        }

        private void ResetCurrentCell()
        {
            if (_currentCell != null)
            {
                _currentCell.ActivatePlaceholder(false);
                _currentCell = null;
            }
        }

        private void OnDestroy()
        {
            InputEventProvider.Instance.PointerDown -= OnPointerDown;
            InputEventProvider.Instance.Drag -= OnDrag;
            InputEventProvider.Instance.PointerUp -= OnPointerUp;
        }
    }
}