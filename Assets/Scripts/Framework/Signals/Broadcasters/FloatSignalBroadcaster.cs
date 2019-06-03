namespace Framework.Signals.Broadcasters
{
    public class FloatSignalBroadcaster : SignalBroadcaster<float>
    {
        public override void Broadcast()
        {
            SignalsManager.Broadcast(Signal.Name, Parameter);
        }
    }
}