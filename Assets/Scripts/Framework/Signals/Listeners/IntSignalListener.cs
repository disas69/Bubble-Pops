using Framework.UnityEvents;

namespace Framework.Signals.Listeners
{
    public class IntSignalListener : SignalListener<int, UnityIntEvent>
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