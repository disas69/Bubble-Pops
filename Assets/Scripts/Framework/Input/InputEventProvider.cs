using System;
using Framework.Extensions;
using Framework.Tools.Singleton;
using UnityEngine.EventSystems;

namespace Framework.Input
{
    public class InputEventProvider : MonoSingleton<InputEventProvider>, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public event Action<PointerEventData> PointerClick;
        public event Action<PointerEventData> PointerEnter;
        public event Action<PointerEventData> PointerExit;
        public event Action<PointerEventData> PointerDown;
        public event Action<PointerEventData> PointerUp;
        public event Action<PointerEventData> BeginDrag;
        public event Action<PointerEventData> Drag;
        public event Action<PointerEventData> EndDrag;

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter.SafeInvoke(eventData);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            PointerExit.SafeInvoke(eventData);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            PointerDown.SafeInvoke(eventData);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            PointerUp.SafeInvoke(eventData);
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag.SafeInvoke(eventData);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            Drag.SafeInvoke(eventData);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            EndDrag.SafeInvoke(eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerClick.SafeInvoke(eventData);
        }
    }
}