using System.Collections;
using UnityEngine;

namespace Game.Utils
{
    public class ColorChanger : MonoBehaviour
    {
        private float _elapsedTime;
        private Coroutine _colorCoroutine;

        [SerializeField] private float _changeTime;
        [SerializeField] private SpriteRenderer _renderer;

        public void SetRenderer(SpriteRenderer spriteRenderer)
        {
            _renderer = spriteRenderer;
        }

        public void ChangeColor(Color color)
        {
            if (_colorCoroutine != null)
            {
                StopCoroutine(_colorCoroutine);
            }

            _elapsedTime = 0f;
            _colorCoroutine = StartCoroutine(ChangeColorCoroutine(_renderer.color, color, _changeTime));
        }

        private IEnumerator ChangeColorCoroutine(Color current, Color target, float changeTime)
        {
            while (_elapsedTime < changeTime)
            {
                _renderer.color = Color.Lerp(current, target, _elapsedTime / changeTime);
                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            _renderer.color = target;
        }
    }
}