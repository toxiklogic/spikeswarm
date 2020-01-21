﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommandSpawner : NetworkBehaviour
{
	// Referene to game data
	public GameData GameData;

	// Reference to command prefab
	public GameObject CommandPrefab;

	// Spawn locations for commands
	public Transform SpawnMin;
	public Transform SpawnMax;

	// Maximum amount of commands allowed on the playing field at once
	public int MaxCommandCount;

    // Minimum and maximum time values for command spawn
    public float RandomSpawnTimeMin;
    public float RandomSpawnTimeMax;

	private List<Command> _spawnedCommands = new List<Command>();

    float _nextSpawnTime;
	int _nextUniqueId = 0;

	private void Start()
	{
		_nextSpawnTime = Time.time + Random.Range(RandomSpawnTimeMin, RandomSpawnTimeMax);
	}

	// Update is called once per frame
	void Update ()
	{
		// Server authoratative
		if (isServer && CanSpawnCommand())
		{
			ServerSpawnCommand();

			_nextSpawnTime = Time.time + Random.Range(RandomSpawnTimeMin, RandomSpawnTimeMax);
		}
	}

	private bool CanSpawnCommand()
	{
		return Time.time > _nextSpawnTime && _spawnedCommands.Count < MaxCommandCount;
	}

	private void ServerSpawnCommand()
	{
		float x = Random.Range(SpawnMin.position.x, SpawnMax.position.x);
		float z = Random.Range(SpawnMin.position.z, SpawnMax.position.z);

		Command.CommandType type = ChooseCommandType();
		RpcSpawnCommand(x, z, type, _nextUniqueId++);
	}

	private Command.CommandType ChooseCommandType()
	{
		// TODO AI
		return (Command.CommandType)Random.Range(0, (int)Command.CommandType.MAX);
	}

	[ClientRpc]
	private void RpcSpawnCommand(float x, float z, Command.CommandType type, int uniqueId)
	{
		GameObject go = GameObject.Instantiate(CommandPrefab, new Vector3(x, 0.0f, z), Quaternion.identity);

		if (go == null)
		{
			Debug.LogError("failed to instantiate command");
			return;
		}

		Command command = go.GetComponent<Command>();

		if (command == null)
		{
			Debug.LogError("failed to instantiate command");
			return;
		}

		for (int i = 0; i < GameData.CommandIconSpriteInfos.Length; i++)
		{
			if (GameData.CommandIconSpriteInfos[i].Type == type)
			{
				command.Setup(GameData.CommandIconSpriteInfos[i].BackgroundColor, GameData.CommandIconSpriteInfos[i].Icon, uniqueId);
				break;
			}
		}

		if (isServer)
		{
			_spawnedCommands.Add(command);
		}
	}
}
