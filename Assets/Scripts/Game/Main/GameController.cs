using System;
using Framework.Tools.Gameplay;
using Framework.Tools.Singleton;
using Framework.UI;
using Game.Data;
using Game.UI;
using UnityEngine;

namespace Game.Main
{
    [RequireComponent(typeof(GameSession))]
    public class GameController : MonoSingleton<GameController>
    {
        private StateMachine<GameState> _stateMachine;

        public GameSession GameSession { get; private set; }

        public GameState GameState
        {
            get { return _stateMachine.CurrentState; }
        }

        protected override void Awake()
        {
            base.Awake();
            GameSession = GetComponent<GameSession>();
        }

        private void Start()
        {
            GameData.Load();
            CreateStateMachine();
            ActivateStartState();
        }

        public void SetState(string stateName)
        {
            SetState((GameState) Enum.Parse(typeof(GameState), stateName));
        }

        public void SetState(GameState gameState)
        {
            _stateMachine.SetState(gameState);
        }

        private void CreateStateMachine()
        {
            _stateMachine = new StateMachine<GameState>(GameState.Start);
            _stateMachine.AddTransition(GameState.Start, GameState.Play, ActivatePlayState);
            _stateMachine.AddTransition(GameState.Play, GameState.Pause, ActivatePauseState);
            _stateMachine.AddTransition(GameState.Pause, GameState.Play, ActivatePlayState);
        }

        private void ActivateStartState()
        {
            GameSession.ResetSession();
            NavigationManager.Instance.OpenScreen<StartPage>();
        }

        private void ActivatePlayState()
        {
            GameSession.StartSession();
            NavigationManager.Instance.OpenScreen<PlayPage>();
        }

        private void ActivatePauseState()
        {
            GameSession.StopSession();
            //TODO: Open pause popup
        }

        private void OnDestroy()
        {
            GameData.Save();
        }
    }
}