using Framework.UnityEvents;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Signals
{
    public static class SignalsManager
    {
        private static readonly SignalHandler SignalHandler;
        private static readonly SignalHandler<string, UnityStringEvent> StringSignalHandler;
        private static readonly SignalHandler<float, UnityFloatEvent> FloatSignalHandler;
        private static readonly SignalHandler<int, UnityIntEvent> IntSignalHandler;
        private static readonly SignalHandler<Vector3, UnityVector3Event> Vector3SignalHandler;

        static SignalsManager()
        {
            SignalHandler = new SignalHandler();
            StringSignalHandler = new SignalHandler<string, UnityStringEvent>();
            FloatSignalHandler = new SignalHandler<float, UnityFloatEvent>();
            IntSignalHandler = new SignalHandler<int, UnityIntEvent>();
            Vector3SignalHandler = new SignalHandler<Vector3, UnityVector3Event>();
        }

        #region Signals

        public static void Register(string signalName, UnityAction action)
        {
            SignalHandler.Register(signalName, action);
        }

        public static void Unregister(string signalName, UnityAction action)
        {
            SignalHandler.Unregister(signalName, action);
        }

        public static void Broadcast(string signalName)
        {
            SignalHandler.Broadcast(signalName);
        }

        #endregion

        #region SrtingSignals

        public static void Register(string signalName, UnityAction<string> action)
        {
            StringSignalHandler.Register(signalName, action);
        }

        public static void Unregister(string signalName, UnityAction<string> action)
        {
            StringSignalHandler.Unregister(signalName, action);
        }

        public static void Broadcast(string signalName, string value)
        {
            StringSignalHandler.Broadcast(signalName, value);
        }

        #endregion

        #region FloatSignals

        public static void Register(string signalName, UnityAction<float> action)
        {
            FloatSignalHandler.Register(signalName, action);
        }

        public static void Unregister(string signalName, UnityAction<float> action)
        {
            FloatSignalHandler.Unregister(signalName, action);
        }

        public static void Broadcast(string signalName, float value)
        {
            FloatSignalHandler.Broadcast(signalName, value);
        }

        #endregion

        #region IntSignals

        public static void Register(string signalName, UnityAction<int> action)
        {
            IntSignalHandler.Register(signalName, action);
        }

        public static void Unregister(string signalName, UnityAction<int> action)
        {
            IntSignalHandler.Unregister(signalName, action);
        }

        public static void Broadcast(string signalName, int value)
        {
            IntSignalHandler.Broadcast(signalName, value);
        }

        #endregion

        #region Vector3Signals

        public static void Register(string signalName, UnityAction<Vector3> action)
        {
            Vector3SignalHandler.Register(signalName, action);
        }

        public static void Unregister(string signalName, UnityAction<Vector3> action)
        {
            Vector3SignalHandler.Unregister(signalName, action);
        }

        public static void Broadcast(string signalName, Vector3 value)
        {
            Vector3SignalHandler.Broadcast(signalName, value);
        }

        #endregion
    }
}