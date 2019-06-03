using System;
using System.Collections.Generic;
using Framework.UI.Structure.Base.View;
using UnityEngine;
using Screen = Framework.UI.Structure.Base.Screen;

namespace Framework.UI.Configuration
{
    [CreateAssetMenu(fileName = "ScreensMapping", menuName = "UI/ScreensMapping")]
    public class ScreensMapping : ScriptableObject
    {
        public List<ScreenSetting> ScreenSettings = new List<ScreenSetting>();
        public List<PopupSettings> PopupSettings = new List<PopupSettings>();
    }

    [Serializable]
    public class ScreenSetting
    {
        public Screen Screen;
        public bool IsCached;
    }

    [Serializable]
    public class PopupSettings
    {
        public Popup Popup;
    }
}