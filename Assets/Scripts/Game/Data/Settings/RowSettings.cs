using System;
using UnityEngine;

namespace Game.Data.Settings
{
    [Serializable]
    public class RowSettings
    {
        public float RowShift;
        public float RowHeight;
        public float MoveTime;
        public float MoveStep;
        public AnimationCurve MoveCurve;
    }
}