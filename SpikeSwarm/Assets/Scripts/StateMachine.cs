using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
	private List<State> _states;
	private State _currentState = null;

	public class State
	{
		public virtual void Enter() { }
		public virtual void Update() { }
		public virtual void Exit() { }
	}

	public void AddState(State state)
	{
		if (state == null)
			return;

		_states.Add(state);
	}

	public void ChangeState<T>() where T : State, new()
	{
		State state = _states.Find(x => x.GetType() == typeof(T));

		// State does not exist yet, add it
		if(state == null)
		{
			state = new T();
			_states.Add(state);
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

	}
}
