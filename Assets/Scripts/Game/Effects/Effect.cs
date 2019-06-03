using Game.Data;
using Game.Spawn;
using UnityEngine;

namespace Game.Effects
{
    public class Effect : SpawnableObject
    {
        private bool _isActive;

        [SerializeField] private Material _material;
        [SerializeField] private ParticleSystem _particleSystem;
        
        public void Play(int color)
        {
            _isActive = true;
            _material.color = GameConfiguration.GetColor(color);
            _particleSystem.Play();
        }

        private void Update()
        {
            if (_isActive)
            {
                if (!_particleSystem.isPlaying)
                {
                    _isActive = false;
                    Deactivate();
                }
            }
        }
    }
}