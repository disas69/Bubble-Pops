using System.Collections.Generic;
using Framework.Audio.Configuration;
using Framework.Tools.Gameplay;
using UnityEngine;

namespace Framework.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        private List<AudioSource> _activeAudioSources;
        private Pool<AudioSource> _audioSourcePool;

        [SerializeField] private AudioStorage _audioStorage;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private int _audioPoolCapacity;
        [SerializeField] private bool _checkCapacity;

        private void Awake()
        {
            _activeAudioSources = new List<AudioSource>();
            _audioSourcePool = new Pool<AudioSource>(_audioSource, transform, _audioPoolCapacity);
        }

        public void Play(string audioKey)
        {
            var audioClip = _audioStorage.GetAudioClip(audioKey);
            if (audioClip != null)
            {
                if (_checkCapacity)
                {
                    var activeAudio = _activeAudioSources.FindAll(s => s.clip == audioClip);
                    if (activeAudio.Count >= _audioStorage.GetCapacity(audioKey))
                    {
                        return;
                    }
                }
                
                var audioSource = _audioSourcePool.GetNext();
                audioSource.clip = audioClip;
                audioSource.Play();

                _activeAudioSources.Add(audioSource);
            }
        }

        private void Update()
        {
            for (var i = _activeAudioSources.Count - 1; i >= 0; i--)
            {
                var audioSource = _activeAudioSources[i];
                if (!audioSource.isPlaying)
                {
                    _audioSourcePool.Return(audioSource);
                    _activeAudioSources.RemoveAt(i);
                }
            }
        }

        public void OnDestroy()
        {
            for (int i = 0; i < _activeAudioSources.Count; i++)
            {
                _audioSourcePool.Return(_activeAudioSources[i]);
            }

            _activeAudioSources.Clear();
            _audioSourcePool.Dispose();
        }
    }
}