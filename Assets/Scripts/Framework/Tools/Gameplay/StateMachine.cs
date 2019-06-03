using System;
using System.Collections.Generic;

namespace Framework.Tools.Gameplay
{
    public class StateMachine<T> where T : struct, IConvertible
    {
        private readonly Dictionary<T, Dictionary<T, Action>> _transitions;

        public T CurrentState { get; private set; }

        public StateMachine()
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception(string.Format("{0} is not an enum. Can't create a state machine", typeof(T).Name));
            }

            _transitions = new Dictionary<T, Dictionary<T, Action>>();

            var states = (T[]) Enum.GetValues(typeof(T));
            CurrentState = states[0];
        }

        public StateMachine(T initialState) : this()
        {
            CurrentState = initialState;
        }

        public void AddTransition(T stateA, T stateB, Action action)
        {
            Dictionary<T, Action> stateTransitions;
            if (!_transitions.TryGetValue(stateA, out stateTransitions))
            {
                stateTransitions = new Dictionary<T, Action>();
                _transitions[stateA] = stateTransitions;
            }

            stateTransitions[stateB] = action;
        }

        public void SetState(T state)
        {
            Action action = null;
            Dictionary<T, Action> stateTransitions;
            if (_transitions.TryGetValue(CurrentState, out stateTransitions))
            {
                stateTransitions.TryGetValue(state, out action);
            }

            CurrentState = state;

            if (action != null)
            {
                action.Invoke();
            }
        }

        public bool IsInState(T state)
        {
            return CurrentState.Equals(state);
        }
    }
}