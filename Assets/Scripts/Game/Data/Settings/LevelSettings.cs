using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data.Settings
{
    [Serializable]
    public class LevelSettings
    {
        public int Level;
        public int Score;

        public void Copy(LevelSettings levelSettings)
        {
            Level = levelSettings.Level;
            Score = levelSettings.Score;
        }
    }
}