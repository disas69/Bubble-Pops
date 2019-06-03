using System;
using Game.Spawn;

namespace Game.Data.Settings
{
    [Serializable]
    public class SpawnerSettings
    {
        public SpawnableObject ObjectPrefab;
        public int PoolCapacity;
    }
}