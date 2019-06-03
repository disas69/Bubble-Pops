using Framework.UnityEvents;
using UnityEngine.Events;

namespace Framework.Signals.Listeners
{
    public class FloatSignalListener : SignalListener<float, UnityFloatEvent>
    {
        protected override void Register()
        {
            SignalsManager.Register(Signal.Name, (UnityAction<float>) Action.Invoke);
        }

        protected override void Unregister()
        {
            SignalsManager.Unregister(Signal.Name, (UnityAction<float>) Action.Invoke);
        }
    }
}