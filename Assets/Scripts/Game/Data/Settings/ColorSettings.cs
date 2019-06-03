using System;
using UnityEngine;

namespace Game.Data.Settings
{
    [Serializable]
    public class ColorSettings
    {
        public int Value;
        public Color Color;
        [Range(0f, 1f)]
        public float Chance;
    }
}