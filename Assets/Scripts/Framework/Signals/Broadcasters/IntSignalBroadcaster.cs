namespace Framework.Signals.Broadcasters
{
    public class IntSignalBroadcaster : SignalBroadcaster<int>
    {
        public override void Broadcast()
        {
            SignalsManager.Broadcast(Signal.Name, Parameter);
        }
    }
}