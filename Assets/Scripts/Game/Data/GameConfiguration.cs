using System.Collections.Generic;
using Framework.Attributes;
using Framework.Tools.Singleton;
using Game.Data.Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Data
{
    [ResourcePath("GameConfiguration")]
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Game/GameConfiguration")]
    public class GameConfiguration : ScriptableSingleton<GameConfiguration>
    {
        public GridSettings GridSettings;
        public RowSettings RowSettings;
        public PopSettings PopSettings;
        public List<EffectsSettings> Effects = new List<EffectsSettings>();
        public List<ColorSettings> Colors = new List<ColorSettings>();
        public List<LevelSettings> Levels = new List<LevelSettings>();

        public static int GetRandomColor(int level)
        {
            var levelSettings = GetLevelSettings(level);
            if (levelSettings != null)
            {
                var maxIndex = 0;
                var random = Random.value;

                var colors = Instance.Colors;
                for (var i = 0; i < colors.Count; i++)
                {
                    if (colors[i].Chance >= random)
                    {
                        maxIndex++;
                    }
                }
                
                var index = Random.Range(0, maxIndex);
                if (colors.Count > index)
                {
                    return colors[index].Value;
                }
            }

            return 0;
        }

        public static Color GetColor(int value)
        {
            var settings = Instance.Colors.Find(c => c.Value == value);
            if (settings != null)
            {
                return settings.Color;
            }

            Debug.LogWarning($"Failed to find color for value: {value}");
            return Color.black;
        }

        public static LevelSettings GetLevelSettings(int level)
        {
            var settings = Instance.Levels.Find(l => l.Level == level);
            if (settings != null)
            {
                return settings;
            }

            Debug.LogWarning($"Failed to find level settings for level: {level}");
            return null;
        }
        
        public static int GetLevelByScore(int score)
        {
            var settings = Instance.Levels.Find(l => l.Score > score);
            if (settings != null)
            {
                return settings.Level;
            }

            if (Instance.Levels.Count > 0)
            {
                return Instance.Levels[Instance.Levels.Count - 1].Level;
            }

            Debug.LogWarning($"Failed to find level for score: {score}");
            return 1;
        }
    }
}