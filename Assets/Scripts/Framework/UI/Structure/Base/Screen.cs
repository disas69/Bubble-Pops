using System;
using System.Collections;
using Framework.Extensions;
using UnityEngine;

namespace Framework.UI.Structure.Base
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Screen : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Coroutine _transitionCoroutine;
        private INavigationManager _navigationManager;

        [SerializeField] private bool _inTransition = true;
        [SerializeField] private bool _outTransition = true;
        [SerializeField] private float _transitionSpeed = 1f;

        public bool IsInTransition
        {
            get { return _transitionCoroutine != null; }
        }

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Initialize(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public virtual void Close()
        {
            _navigationManager.Back();
        }

        public virtual void OnEnter()
        {
            gameObject.SetActive(true);

            this.SafeStopCoroutine(_transitionCoroutine);
            _transitionCoroutine = null;

            if (_inTransition)
            {
                _transitionCoroutine = StartCoroutine(InTransition(Activate));
            }
            else
            {
                Activate();
            }
        }

        public virtual void OnExit()
        {
            this.SafeStopCoroutine(_transitionCoroutine);
            _transitionCoroutine = null;

            if (_outTransition)
            {
                _transitionCoroutine = StartCoroutine(OutTransition(Deactivate));
            }
            else
            {
                Deactivate();
            }
        }

        protected virtual void Activate()
        {
        }

        protected virtual void Deactivate()
        {
            gameObject.SetActive(false);
        }

        protected virtual IEnumerator InTransition(Action callback)
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            _canvasGroup.alpha = 0f;

            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += _transitionSpeed * Time.deltaTime;
                yield return null;
            }

            _transitionCoroutine = null;
            callback.SafeInvoke();
        }

        protected virtual IEnumerator OutTransition(Action callback)
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            _canvasGroup.alpha = 1f;

            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= _transitionSpeed * Time.deltaTime;
                yield return null;
            }

            _transitionCoroutine = null;
            callback.SafeInvoke();
        }
    }
}