using System;
using System.Collections;
using Framework.Signals;
using Game.Data;
using Game.Effects;
using Game.Main;
using Game.Spawn;
using Game.Utils;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Objects
{
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
    public class Pop : SpawnableObject
    {
        private const string FallZoneTag = "FallZone";
        
        private readonly int _appearHash = Animator.StringToHash("Appear");
        private readonly int _mergeHash = Animator.StringToHash("Merge");
        private readonly int _reactHash = Animator.StringToHash("React");
        
        private int _value;
        private float _elapsedTime;
        private Vector3 _currentPosition;
        private Animator _animator;
        private Rigidbody2D _rigidbody2D;
        private Collider2D _collider2D;

        [SerializeField] private SpriteRenderer _backSprite;
        [SerializeField] private SpriteRenderer _kSprite;
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private ColorChanger _colorChanger;
        [SerializeField] private Signal _audioSignal;

        public int Value => _value;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.keepAnimatorControllerStateOnDisable = true;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
        }

        public void Setup(int value)
        {
            _value = value;
            _valueText.text = FormatHelper.FormatValue(value, true);
            
            if (value < 1000)
            {
                _kSprite.enabled = false;
                _colorChanger.SetRenderer(_backSprite);
            }
            else
            {
                _kSprite.enabled = true;
                _backSprite.color = Color.white;
                _colorChanger.SetRenderer(_kSprite);
            }

            _colorChanger.ChangeColor(GameConfiguration.GetColor(value));
            _animator.SetTrigger(_appearHash);
            
            EnablePhysics(false);
        }

        public void React()
        {
            _animator.SetTrigger(_reactHash);
            EffectsManager.Instance.Play(GameConfiguration.Instance.PopSettings.ReactEffect, _value, transform.position);
        }

        public void Move(Cell cell, Action callback = null)
        {
            Move(cell, GameConfiguration.Instance.PopSettings.MoveTime, callback);
        }

        public void Move(Cell cell, float time, Action callback = null)
        {
            _elapsedTime = 0;
            _currentPosition = transform.position;
            StartCoroutine(MoveRoutine(cell, time, GameConfiguration.Instance.PopSettings.MoveCurve, callback));
        }

        public void Merge(Cell cell, Action callback = null)
        {
            _elapsedTime = 0;
            _currentPosition = transform.position;
            _animator.SetTrigger(_mergeHash);
            EffectsManager.Instance.Play(GameConfiguration.Instance.PopSettings.MergeEffect, _value, transform.position);
            StartCoroutine(MoveRoutine(cell, GameConfiguration.Instance.PopSettings.MergeTime, GameConfiguration.Instance.PopSettings.MergeCurve, callback));
        }

        public void Enhance(int level)
        {
            var value = _value;
            for (var i = 1; i < level; i++)
            {
                value *= 2;
            }
            
            GameController.Instance.GameSession.AddScorePoints(value);
            Setup(value);
            
            SignalsManager.Broadcast(_audioSignal.Name, "merge");
            EffectsManager.Instance.Play(GameConfiguration.Instance.PopSettings.MergeEffect, _value, transform.position);
        }

        public void Detach()
        {
            EnablePhysics(true);
            _rigidbody2D.AddForce(Vector2.up * Random.Range(GameConfiguration.Instance.PopSettings.MinUpForce, GameConfiguration.Instance.PopSettings.MaxUpForce), ForceMode2D.Impulse);
        }

        public void BlowUp()
        {
            GameController.Instance.GameSession.AddScorePoints(_value);
            SignalsManager.Broadcast(_audioSignal.Name, "splash");
            EffectsManager.Instance.Play(GameConfiguration.Instance.PopSettings.BlowUpEffect, _value, transform.position);
            Deactivate();
        }

        public override void Deactivate()
        {
            EnablePhysics(false);
            ResetTriggers();
            base.Deactivate();
        }

        public void EnablePhysics(bool enable)
        {
            _collider2D.enabled = enable;
            _rigidbody2D.isKinematic = !enable;
            _rigidbody2D.velocity = Vector2.zero;
        }

        private void ResetTriggers()
        {
            _animator.ResetTrigger(_appearHash);
            _animator.ResetTrigger(_mergeHash);
            _animator.ResetTrigger(_reactHash);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(FallZoneTag))
            {
                BlowUp();
            }
        }

        private IEnumerator MoveRoutine(Cell cell, float time, AnimationCurve curve, Action callback = null)
        {
            while (_elapsedTime < time)
            {
                transform.position = Vector3.Lerp(_currentPosition, cell.transform.position, curve.Evaluate(_elapsedTime / time));
                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = cell.transform.position;
            callback?.Invoke();
        }
    }
}