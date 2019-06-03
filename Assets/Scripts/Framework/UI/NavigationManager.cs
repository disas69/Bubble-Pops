using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Extensions;
using Framework.Tools.Singleton;
using Framework.UI.Configuration;
using Framework.UI.Structure;
using Framework.UI.Structure.Base.View;
using UnityEngine;
using Screen = Framework.UI.Structure.Base.Screen;

namespace Framework.UI
{
    public class NavigationManager : MonoSingleton<NavigationManager>, INavigationManager
    {
        private bool _isInitialized;
        private Dictionary<Type, Screen> _cachedScreens;
        private Dictionary<Type, ScreenSetting> _screenSettingsDictionary;
        private Dictionary<Type, PopupSettings> _popupSettingsDictionary;
        private Coroutine _screenOpeningCoroutine;
        private Coroutine _popupShowingCoroutine;
        private Type _previousScreenType;

        [SerializeField] private ScreensMapping _screensMapping;
        [SerializeField] private Transform _pagesRoot;
        [SerializeField] private Transform _popupsRoot;

        public Screen CurrentScreen { get; private set; }

        public void OpenScreen<T>() where T : Screen
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            this.SafeStopCoroutine(_screenOpeningCoroutine);
            _screenOpeningCoroutine = StartCoroutine(OpenScreen(typeof(T)));
        }

        public void ShowPopup<T>() where T : Popup
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            this.SafeStopCoroutine(_popupShowingCoroutine);
            _popupShowingCoroutine = StartCoroutine(ShowPopup(typeof(T)));
        }

        public void Back()
        {
            if (_previousScreenType != null)
            {
                this.SafeStopCoroutine(_screenOpeningCoroutine);
                _screenOpeningCoroutine = StartCoroutine(OpenScreen(_previousScreenType));
            }
            else
            {
                Debug.LogWarning("Attempt to navigate Back when there is no previous screen");
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            _cachedScreens = new Dictionary<Type, Screen>();
            _screenSettingsDictionary = new Dictionary<Type, ScreenSetting>(_screensMapping.ScreenSettings.Count);
            _popupSettingsDictionary = new Dictionary<Type, PopupSettings>(_screensMapping.PopupSettings.Count);

            for (int i = 0; i < _screensMapping.ScreenSettings.Count; i++)
            {
                var screenSettings = _screensMapping.ScreenSettings[i];
                if (screenSettings != null)
                {
                    _screenSettingsDictionary.Add(screenSettings.Screen.GetType(), screenSettings);
                }
            }

            for (int i = 0; i < _screensMapping.PopupSettings.Count; i++)
            {
                var popupSettings = _screensMapping.PopupSettings[i];
                if (popupSettings != null)
                {
                    _popupSettingsDictionary.Add(popupSettings.Popup.GetType(), popupSettings);
                }
            }

            _isInitialized = true;
        }

        private IEnumerator OpenScreen(Type screenType)
        {
            if (CurrentScreen != null)
            {
                _previousScreenType = CurrentScreen.GetType();

                CurrentScreen.OnExit();
                while (CurrentScreen.IsInTransition)
                {
                    yield return null;
                }

                Screen currentScreen;
                if (!_cachedScreens.TryGetValue(CurrentScreen.GetType(), out currentScreen))
                {
                    Destroy(CurrentScreen.gameObject);
                }
            }

            Screen cachedScreen;
            if (_cachedScreens.TryGetValue(screenType, out cachedScreen))
            {
                cachedScreen.OnEnter();
                CurrentScreen = cachedScreen;
            }
            else
            {
                ScreenSetting screenSetting;
                if (_screenSettingsDictionary.TryGetValue(screenType, out screenSetting))
                {
                    var screen = Instantiate(screenSetting.Screen, _pagesRoot);
                    screen.Initialize(this);

                    if (screenSetting.IsCached)
                    {
                        _cachedScreens.Add(screen.GetType(), screen);
                    }

                    screen.OnEnter();
                    CurrentScreen = screen;
                }
                else
                {
                    throw new NotImplementedException(string.Format("Screen of type {0} isn't implemented yet", screenType.Name));
                }
            }

            while (CurrentScreen.IsInTransition)
            {
                yield return null;
            }

            _screenOpeningCoroutine = null;
        }

        private IEnumerator ShowPopup(Type popupType)
        {
            PopupSettings popupSettings;
            if (_popupSettingsDictionary.TryGetValue(popupType, out popupSettings))
            {
                var popup = Instantiate(popupSettings.Popup, _popupsRoot);
                popup.OnEnter();

                while (popup.IsInTransition)
                {
                    yield return null;
                }
            }
        }
    }
}