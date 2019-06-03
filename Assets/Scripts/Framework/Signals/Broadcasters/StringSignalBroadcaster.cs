namespace Framework.Signals.Broadcasters
{
    public class StringSignalBroadcaster : SignalBroadcaster<string>
    {
        public override void Broadcast()
        {
            SignalsManager.Broadcast(Signal.Name, Parameter);
        }
    }
}