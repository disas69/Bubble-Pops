using System;
using System.Collections.Generic;
using Framework.Tools.Singleton;
using Game.Data;
using Game.Data.Settings;
using Game.Spawn;
using UnityEngine;

namespace Game.Effects
{
    [Serializable]
    public class EffectSpawnConfig
    {
        public string Effect;
        public Spawner Spawner;
    }

    public class EffectsManager : MonoSingleton<EffectsManager>
    {
        private List<EffectSpawnConfig> _effects = new List<EffectSpawnConfig>();

        [SerializeField] private int _poolsCapacity;

        protected override void Awake()
        {
            base.Awake();

            var settings = GameConfiguration.Instance.Effects;
            for (var i = 0; i < settings.Count; i++)
            {
                var effect = settings[i];
                var spawnSettings = new SpawnerSettings
                {
                    ObjectPrefab = effect.Prefab, PoolCapacity = _poolsCapacity
                };

                var spawner = new GameObject(string.Format("Spawner [{0}]", effect.Name)).AddComponent<Spawner>();
                spawner.transform.SetParent(transform);
                spawner.Activate(spawnSettings);

                _effects.Add(new EffectSpawnConfig {Effect = effect.Name, Spawner = spawner});
            }
        }

        public void Play(string effectName, int color, Vector3 position)
        {
            var spawnConfig = _effects.Find(c => c.Effect == effectName);
            if (spawnConfig != null)
            {
                var effect = spawnConfig.Spawner.Spawn() as Effect;
                if (effect != null)
                {
                    effect.transform.SetParent(transform);
                    effect.transform.position = position;
                    effect.Play(color);
                }
            }
            else
            {
                Debug.LogError($"Failed to find effect config by name {effectName}");
            }
        }
    }
}