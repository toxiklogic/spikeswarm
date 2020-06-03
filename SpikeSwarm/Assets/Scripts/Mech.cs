using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech : MonoBehaviour
{
    public class MechState : StateMachine.State
    {
        protected Mech _context;
        public MechState(Mech context) { _context = context; }
    }

    public class IdleState : MechState
    {
        public IdleState(Mech context) : base(context) { }
    }

    public class MoveState : MechState
    {
        private float _moveTime;

        public MoveState(Mech context) : base(context) { }

        public override void Enter()
        {
            _moveTime = 0.0f;
            _context.AnimationController.Walk();
        }

        public override void Update()
        {
            _moveTime += Time.deltaTime;

            if (_moveTime > _context.MoveDuration)
            {
                _context._stateMachine.ChangeState<IdleState>();
                return;
            }

            if(_context._currentCommand == Command.CommandType.DIR_UP)
            {
                _context.transform.position += Vector3.forward * _context.MoveSpeed * Time.deltaTime;
            }
            else if (_context._currentCommand == Command.CommandType.DIR_DOWN)
            {
                _context.transform.position -= Vector3.forward * _context.MoveSpeed * Time.deltaTime;
            }
            else if (_context._currentCommand == Command.CommandType.DIR_LEFT)
            {
                _context.transform.position += Vector3.left * _context.MoveSpeed * Time.deltaTime;
            }
            else if (_context._currentCommand == Command.CommandType.DIR_RIGHT)
            {
                _context.transform.position -= Vector3.left * _context.MoveSpeed * Time.deltaTime;
            }
        }

        public override void Exit()
        {
            _context.AnimationController.Idle();
        }
    }

    public class FireState : MechState
    {
        public FireState(Mech context) : base(context) { }
    }

    public MechAnimController AnimationController;
    public float MoveSpeed = 3.0f;
    public float MoveDuration = 3.0f;

    private StateMachine _stateMachine = new StateMachine();
    private Command.CommandType _currentCommand;

    void Start()
    {
        _stateMachine.AddState(new IdleState(this));
        _stateMachine.AddState(new MoveState(this));
        _stateMachine.AddState(new FireState(this));
        _stateMachine.ChangeState<IdleState>();
    }

    void Update()
    {
        _stateMachine.Update();

        // Debug code
        //if(Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    ExecuteCommand(Command.CommandType.DIR_LEFT);
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    ExecuteCommand(Command.CommandType.DIR_RIGHT);
        //}
        //else if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    ExecuteCommand(Command.CommandType.DIR_UP);
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    ExecuteCommand(Command.CommandType.DIR_DOWN);
        //}
    }

    public void ExecuteCommand(Command.CommandType commandType)
    {
        _currentCommand = commandType;

        switch (commandType)
        {
            case Command.CommandType.DIR_UP:
                transform.rotation = Quaternion.identity;
                _stateMachine.ChangeState<MoveState>();
                break;
            case Command.CommandType.DIR_DOWN:
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                _stateMachine.ChangeState<MoveState>();
                break;
            case Command.CommandType.DIR_LEFT:
                transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                _stateMachine.ChangeState<MoveState>();
                break;
            case Command.CommandType.DIR_RIGHT:
                transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                _stateMachine.ChangeState<MoveState>();
                break;
        }
    }
}
