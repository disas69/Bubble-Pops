using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.Utils
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TapHandler : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private UnityEvent _command;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_command != null)
            {
                _command.Invoke();
            }
        }
    }
}