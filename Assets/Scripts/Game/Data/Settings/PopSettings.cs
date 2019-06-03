using System;
using UnityEngine;

namespace Game.Data.Settings
{
    [Serializable]
    public class PopSettings
    {
        public float MoveTime;
        public AnimationCurve MoveCurve;
        public float MergeTime;
        public AnimationCurve MergeCurve;
        public float MinUpForce;
        public float MaxUpForce;
        public string ReactEffect;
        public string MergeEffect;
        public string BlowUpEffect;
    }
}