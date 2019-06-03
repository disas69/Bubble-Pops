using UnityEngine;

namespace Framework.Signals.Broadcasters
{
    public class SignalBroadcaster : MonoBehaviour
    {
        public Signal Signal;

        public void Broadcast()
        {
            SignalsManager.Broadcast(Signal.Name);
        }
    }

    public abstract class SignalBroadcaster<T> : MonoBehaviour
    {
        public Signal Signal;
        public T Parameter;

        public abstract void Broadcast();
    }
}