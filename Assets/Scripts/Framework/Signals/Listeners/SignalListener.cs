using UnityEngine;
using UnityEngine.Events;

namespace Framework.Signals.Listeners
{
    public class SignalListener : MonoBehaviour
    {
        public Signal Signal;
        public UnityEvent Action;

        private void OnEnable()
        {
            SignalsManager.Register(Signal.Name, Action.Invoke);
        }

        private void OnDisable()
        {
            SignalsManager.Unregister(Signal.Name, Action.Invoke);
        }
    }

    public abstract class SignalListener<T, TEvent> : MonoBehaviour where TEvent : UnityEvent<T>, new()
    {
        public Signal Signal;
        public TEvent Action;

        protected abstract void Register();
        protected abstract void Unregister();

        private void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            Unregister();
        }
    }
}