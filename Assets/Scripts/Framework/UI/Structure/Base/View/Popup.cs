namespace Framework.UI.Structure.Base.View
{
    public class Popup : Screen
    {
        public override void Close()
        {
            OnExit();
        }

        protected override void Deactivate()
        {
            Destroy(gameObject);
        }
    }
}