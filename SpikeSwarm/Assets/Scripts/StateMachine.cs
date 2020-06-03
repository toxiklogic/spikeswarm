using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
	public class State
	{
		public virtual void Enter() { }
		public virtual void Update() { }
		public virtual void Exit() { }
	}

    private List<State> _states = new List<State>();
    private State _currentState = null;

    public State CurrentState { get { return _currentState; } }

    public void AddState(State state)
	{
		if (state == null)
			return;

		_states.Add(state);
	}

	public void ChangeState<T>() where T : State
	{
		State state = _states.Find(x => x.GetType() == typeof(T));

		// State does not exist yet
		if(state == null)
		{
            Debug.LogError(state.ToString() + " does not exist");
		}

		if(_currentState != null)
		{
			_currentState.Exit();
		}

		_currentState = state;
		_currentState.Enter();
	}

	public void ChangeState(State nextState)
	{
		State state = _states.Find(x => x == nextState);

		if (state == null)
			return;

		if (_currentState != null)
		{
			_currentState.Exit();
		}

		_currentState = state;
		_currentState.Enter();
	}

    public void Update()
    {
        if (_currentState != null)
            _currentState.Update();
    }
}
