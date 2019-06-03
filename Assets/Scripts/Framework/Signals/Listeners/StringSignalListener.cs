using Framework.UnityEvents;

namespace Framework.Signals.Listeners
{
    public class StringSignalListener : SignalListener<string, UnityStringEvent>
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