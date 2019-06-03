using System.Collections.Generic;
using UnityEngine.Events;

namespace Framework.Signals
{
    public class SignalHandler
    {
        private readonly Dictionary<string, UnityEvent> _signalsDictionary = new Dictionary<string, UnityEvent>();

        public void Register(string signalName, UnityAction action)
        {
            UnityEvent unityEvent;
            if (_signalsDictionary.TryGetValue(signalName, out unityEvent))
            {
                unityEvent.AddListener(action);
            }
            else
            {
                unityEvent = new UnityEvent();
                unityEvent.AddListener(action);
                _signalsDictionary.Add(signalName, unityEvent);
            }
        }

        public void Unregister(string signalName, UnityAction action)
        {
            UnityEvent unityEvent;
            if (_signalsDictionary.TryGetValue(signalName, out unityEvent))
            {
                unityEvent.RemoveListener(action);
            }
        }

        public void Broadcast(string signalName)
        {
            UnityEvent unityEvent;
            if (_signalsDictionary.TryGetValue(signalName, out unityEvent))
            {
                unityEvent.Invoke();
            }
        }
    }

    public class SignalHandler<T, TEvent> where TEvent : UnityEvent<T>, new()
    {
        private readonly Dictionary<string, TEvent> _signalsDictionary = new Dictionary<string, TEvent>();

        public void Register(string signalName, UnityAction<T> action)
        {
            TEvent unityEvent;
            if (_signalsDictionary.TryGetValue(signalName, out unityEvent))
            {
                unityEvent.AddListener(action);
            }
            else
            {
                unityEvent = new TEvent();
                unityEvent.AddListener(action);
                _signalsDictionary.Add(signalName, unityEvent);
            }
        }

        public void Unregister(string signalName, UnityAction<T> action)
        {
            TEvent unityEvent;
            if (_signalsDictionary.TryGetValue(signalName, out unityEvent))
            {
                unityEvent.RemoveListener(action);
            }
        }

        public void Broadcast(string signalName, T value)
        {
            TEvent unityEvent;
            if (_signalsDictionary.TryGetValue(signalName, out unityEvent))
            {
                unityEvent.Invoke(value);
            }
        }
    }
}