using System;
using System.Collections;
using Framework.Extensions;
using Framework.Signals;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using Game.Main;
using UnityEngine;

namespace Game.UI
{
    public class StartPage : Page<PageModel>
    {
        private Coroutine _overlayTransitionCoroutine;

        [SerializeField] private CanvasGroup _overlay;
        [SerializeField] private float _overlayTransitionSpeed;
        [SerializeField] private Signal _stateChangeSignal;

        public void Play()
        {
            SignalsManager.Broadcast(_stateChangeSignal.Name, GameState.Play.ToString());
        }

        protected override IEnumerator InTransition(Action callback)
        {
            _overlayTransitionCoroutine = StartCoroutine(ShowOverlay());
            yield return _overlayTransitionCoroutine;
        }

        private IEnumerator ShowOverlay()
        {
            _overlay.gameObject.SetActive(true);
            _overlay.alpha = 1f;

            while (_overlay.alpha > 0f)
            {
                _overlay.alpha -= _overlayTransitionSpeed * 2f * Time.deltaTime;
                yield return null;
            }

            _overlay.alpha = 0f;
            _overlay.gameObject.SetActive(false);
            _overlayTransitionCoroutine = null;
        }

        public override void OnExit()
        {
            _overlay.gameObject.SetActive(false);
            this.SafeStopCoroutine(_overlayTransitionCoroutine);
            base.OnExit();
        }
    }
}