using System;
using Framework.Extensions;
using UnityEngine;

namespace Game.Spawn
{
    public class SpawnableObject : MonoBehaviour
    {
        public event Action<SpawnableObject> Deactivated;

        public virtual void Deactivate()
        {
            Deactivated.SafeInvoke(this);
        }
    }
}