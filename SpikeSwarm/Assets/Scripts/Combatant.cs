using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour
{
    public Transform CommandAttachPoint;

    private Command _closestCommand;
    private Command _holdingCommand;

    private class IdleState : StateMachine.State
    {

    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Picking up a command
            if (_closestCommand != null && !IsHoldingCommmand())
            {
                PickupCommand();
            }
            // Dropping a command
            else if (IsHoldingCommmand())
            {
                _holdingCommand.transform.SetParent(null);
                _holdingCommand = null;
            }
        }
    }

    void PickupCommand()
    {
        _closestCommand.transform.SetParent(CommandAttachPoint);
        _closestCommand.transform.localPosition = Vector3.zero;
        _holdingCommand = _closestCommand;
        _closestCommand = null;
    }

    public void OnEnterCommandPickup(Command command)
    {
        if (_closestCommand == null)
            _closestCommand = command;
    }

    public void OnExitCommandPickup(Command command)
    {
        if (_closestCommand == command)
            _closestCommand = null;
    }

    private bool IsHoldingCommmand()
    {
        return _holdingCommand != null;
    }
}
