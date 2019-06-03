using Framework.Signals;
using Game.Data;
using Game.Objects;
using UnityEngine;
using Grid = Game.Objects.Grid;

namespace Game.Main
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private Shooter _shooter;
        [SerializeField] private Signal _levelSignal;
        [SerializeField] private Signal _scoreSignal;

        public int Level { get; private set; }
        public int Score { get; private set; }

        public void ResetSession()
        {
            Level = GameData.Data.Level;
            Score = GameData.Data.Score;
        }

        public void StartSession()
        {
            _grid.Activate();
            _shooter.Activate();
        }

        public void StopSession()
        {
            _grid.Deactivate();
            _shooter.Deactivate();
            SaveLevelAndScore();
        }

        public void AddScorePoints(int scorePoints)
        {
            Score += scorePoints;
            SignalsManager.Broadcast(_scoreSignal.Name, Score);

            var level = GameConfiguration.GetLevelByScore(Score);
            if (level > Level)
            {
                Level = level;
                SignalsManager.Broadcast(_levelSignal.Name, level);
                SaveLevelAndScore();
            }
        }

        private void SaveLevelAndScore()
        {
            GameData.Data.Level = Level;
            GameData.Data.Score = Score;
            GameData.Save();
        }
    }
}