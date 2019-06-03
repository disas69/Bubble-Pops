using Framework.UI.Structure.Base;
using Framework.UI.Structure.Base.View;

namespace Framework.UI.Structure
{
    public interface INavigationManager
    {
        Screen CurrentScreen { get; }
        void OpenScreen<T>() where T : Screen;
        void ShowPopup<T>() where T : Popup;
        void Back();
    }
}