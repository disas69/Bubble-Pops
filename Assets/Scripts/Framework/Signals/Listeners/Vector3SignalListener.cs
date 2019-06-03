using Framework.UnityEvents;
using UnityEngine;

namespace Framework.Signals.Listeners
{
    public class Vector3SignalListener : SignalListener<Vector3, UnityVector3Event>
    {
        protected override void Register()
        {
            SignalsManager.Register(Signal.Name, Action.Invoke);
        }

        protected override void Unregister()
        {
            SignalsManager.Unregister(Signal.Name, Action.Invoke);
        }
    }
}