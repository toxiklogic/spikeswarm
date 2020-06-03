using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Combatant : NetworkBehaviour
{
    public Transform CommandAttachPoint;
    public Mech Mech;

    private Command _closestCommand;
    private Command _holdingCommand;

    private class IdleState : StateMachine.State
    {

    }

    void Start()
    {
		if(isServer)
		{
			Arena.Instance.ServerOnCombatantJoined(this);
		}
    }

    void Update()
    {
        if (!isLocalPlayer || !Arena.Instance.GameStarted)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Picking up a command
            if (_closestCommand != null && !IsHoldingCommmand())
            {
				//PickupCommand();
				CmdPickupCommand(_closestCommand.UniqueId);
            }
            // Dropping a command
            else if (IsHoldingCommmand())
            {
				//_holdingCommand.transform.SetParent(null);
				//_holdingCommand = null;
				CmdDropCommand();
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

	void PickupCommand(Command command)
	{
		_closestCommand = command;
		PickupCommand();
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

	[Command]
	private void CmdPickupCommand(int commandId)
	{
		RpcPickupCommand(commandId);
	}

	[ClientRpc]
	private void RpcPickupCommand(int commandId)
	{
		// Server checks if we can pickup this command
		Command[] commands = FindObjectsOfType<Command>();
		foreach (Command command in commands)
		{
			if (command.UniqueId == commandId)
			{
				PickupCommand(command);
			}
		}
	}

	[Command]
	private void CmdDropCommand()
	{
		RpcDropCommand();
	}

	[ClientRpc]
	private void RpcDropCommand()
	{
		_holdingCommand.transform.SetParent(null);
        _holdingCommand.Drop();
		_holdingCommand = null;
	}
}
