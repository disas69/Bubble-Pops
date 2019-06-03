using Framework.Signals;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using Game.Data;
using Game.Data.Settings;
using Game.Main;
using Game.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PlayPage : Page<PageModel>
    {
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _currentLevel;
        [SerializeField] private TextMeshProUGUI _nextLevel;
        [SerializeField] private Image _progress;
        [SerializeField] private Signal _levelSignal;
        [SerializeField] private Signal _scoreSignal;
        [SerializeField] private Signal _audioSignal;

        public override void OnEnter()
        {
            base.OnEnter();

            UpdateLevel(GameController.Instance.GameSession.Level, GameController.Instance.GameSession.Score);

            SignalsManager.Register(_levelSignal.Name, OnLevelChange);
            SignalsManager.Register(_scoreSignal.Name, OnScoreChanged);
        }

        private void OnLevelChange(int level)
        {
            UpdateLevel(level, GameController.Instance.GameSession.Score);
            SignalsManager.Broadcast(_audioSignal.Name, "new_level");
        }

        private void OnScoreChanged(int score)
        {
            UpdateLevel(GameController.Instance.GameSession.Level, score);
        }

        private void UpdateLevel(int level, int score)
        {
            _score.text = FormatHelper.FormatValue(score);

            var current = GameConfiguration.GetLevelSettings(level);
            if (current != null)
            {
                _currentLevel.gameObject.SetActive(true);
                _currentLevel.text = level.ToString();
                UpdateLevelProgress(score, current);

                var nextLevel = level + 1;
                var next = GameConfiguration.GetLevelSettings(nextLevel);
                if (next != null)
                {
                    _nextLevel.gameObject.SetActive(true);
                    _nextLevel.text = nextLevel.ToString();
                }
                else
                {
                    _nextLevel.gameObject.SetActive(false);
                }
            }
            else
            {
                _currentLevel.gameObject.SetActive(false);
                _nextLevel.gameObject.SetActive(false);
            }
        }

        private void UpdateLevelProgress(int score, LevelSettings current)
        {
            var previousLevelScore = 0;
            var previousLevel = GameConfiguration.GetLevelSettings(current.Level - 1);
            if (previousLevel != null)
            {
                previousLevelScore = previousLevel.Score;
            }
            
            _progress.fillAmount = (float)(score - previousLevelScore) / (current.Score - previousLevelScore);
        }

        public override void OnExit()
        {
            base.OnExit();
            
            SignalsManager.Unregister(_levelSignal.Name, OnLevelChange);
            SignalsManager.Unregister(_scoreSignal.Name, OnScoreChanged);
        }
    }
}