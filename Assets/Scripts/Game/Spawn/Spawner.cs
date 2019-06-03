using System;
using System.Collections.Generic;
using Framework.Tools.Gameplay;
using Game.Data.Settings;
using UnityEngine;

namespace Game.Spawn
{
    [Serializable]
    public class Spawner : MonoBehaviour
    {
        private Pool<SpawnableObject> _objectsPool;
        private List<SpawnableObject> _activeObjects;

        [SerializeField] private bool _activateOnAwake = true;
        [SerializeField] private SpawnerSettings _settings;
        
        public int Count
        {
            get { return _activeObjects.Count; }
        }

        private void Awake()
        {
            _activeObjects = new List<SpawnableObject>();

            if (_activateOnAwake && _settings != null)
            {
                Activate(_settings);
            }
        }

        public void Activate(SpawnerSettings spawnerSettings)
        {
            if (_objectsPool == null)
            {
                _settings = spawnerSettings;
                _objectsPool = new Pool<SpawnableObject>(spawnerSettings.ObjectPrefab, transform, spawnerSettings.PoolCapacity);
            }
        }

        public SpawnableObject Spawn()
        {
            var spawnableObject = _objectsPool.GetNext();
            spawnableObject.Deactivated += OnObjectDeactivated;
            _activeObjects.Add(spawnableObject);
            
            return spawnableObject;
        }

        public void Flush()
        {
            for (var i = 0; i < _activeObjects.Count; i++)
            {
                _activeObjects[i].Deactivate();
            }
        }

        private void OnObjectDeactivated(SpawnableObject spawnableObject)
        {
            Despawn(spawnableObject);
        }

        private void Despawn(SpawnableObject spawnableObject)
        {
            spawnableObject.Deactivated -= OnObjectDeactivated;

            _activeObjects.Remove(spawnableObject);
            _objectsPool.Return(spawnableObject);
        }
    }
}