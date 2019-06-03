using UnityEngine;

namespace Framework.Signals.Broadcasters
{
    public class Vector3SignalBroadcaster : SignalBroadcaster<Vector3>
    {
        public override void Broadcast()
        {
            SignalsManager.Broadcast(Signal.Name, Parameter);
        }
    }
}