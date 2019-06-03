using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Audio.Configuration
{
    [CreateAssetMenu(fileName = "AudioStorage", menuName = "Audio/AudioStorage")]
    public class AudioStorage : ScriptableObject
    {
        private Dictionary<string, AudioClip> _audioDictionary;
        [NonSerialized] private bool _isInitialized;

        public List<AudioClipConfig> AudioClips = new List<AudioClipConfig>();

        public AudioClip GetAudioClip(string key)
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            AudioClip audioClip;
            if (_audioDictionary.TryGetValue(key, out audioClip))
            {
                return audioClip;
            }

            Debug.LogError(string.Format("Failed to find Audio Clip by key {0}", key));
            return null;
        }

        public int GetCapacity(string key)
        {
            var config = AudioClips.Find(a => a.Key == key);
            if (config != null)
            {
                return config.Capacity;
            }

            return 0;
        }

        private void Initialize()
        {
            _audioDictionary = new Dictionary<string, AudioClip>();

            for (int i = 0; i < AudioClips.Count; i++)
            {
                var config = AudioClips[i];
                _audioDictionary.Add(config.Key, config.AudioClip);
            }

            _isInitialized = true;
        }
    }

    [Serializable]
    public class AudioClipConfig
    {
        public string Key;
        public AudioClip AudioClip;
        public int Capacity;
    }
}