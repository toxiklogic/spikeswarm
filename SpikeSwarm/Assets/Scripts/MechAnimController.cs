using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechAnimController : MonoBehaviour
{
	private StateMachine _mechAnimStateMachine = new StateMachine();
	private MechIdleState _idleState;
	private MechWalkState _walkState;

	public class MechAnimState : StateMachine.State
	{
		protected Animator _animator;

		public MechAnimState(Animator animator)
		{
			_animator = animator;
		}
	}

	public class MechIdleState : MechAnimState
	{
		public MechIdleState(Animator animator) : base(animator)
		{
		}

		public override void Enter()
		{
			base.Enter();

			_animator.SetTrigger("idle");
		}
	}

	public class MechWalkState : MechAnimState
	{
		public MechWalkState(Animator animator) : base(animator)
		{
		}

		public override void Enter()
		{
			base.Enter();

			_animator.SetTrigger("walk");
		}
	}


	// Use this for initialization
	void Start()
	{

		Animator animator = GetComponent<Animator>();

		_idleState = new MechIdleState(animator);
		_walkState = new MechWalkState(animator);

		_mechAnimStateMachine.AddState(_idleState);
		_mechAnimStateMachine.AddState(_walkState);
	}

	// Update is called once per frame
	void Update()
	{

		if (Input.GetKeyDown(KeyCode.M))
		{
			_mechAnimStateMachine.ChangeState(_idleState);
		}

		if (Input.GetKeyDown(KeyCode.N))
		{
			_mechAnimStateMachine.ChangeState(_walkState);
		}
	}
}
