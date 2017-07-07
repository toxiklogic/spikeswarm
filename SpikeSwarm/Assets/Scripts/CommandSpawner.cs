using System.Collections;
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

	float _nextSpawnTime;

	private void Start()
	{
		_nextSpawnTime = Time.time + Random.Range(0.5f, 1.0f);
	}

	// Update is called once per frame
	void Update ()
	{
		// Server authoratative
		if (isServer && Time.time > _nextSpawnTime)
		{
			ServerSpawnCommand();

			_nextSpawnTime = Time.time + Random.Range(0.5f, 1.0f);
		}
	}

	private void ServerSpawnCommand()
	{
		float x = Random.Range(SpawnMin.position.x, SpawnMax.position.x);
		float z = Random.Range(SpawnMin.position.z, SpawnMax.position.z);

		Command.CommandType type = ChooseCommandType();
		RpcSpawnCommand(x, z, type);
	}

	private Command.CommandType ChooseCommandType()
	{
		// TODO AI
		return (Command.CommandType)Random.Range(0, (int)Command.CommandType.MAX);
	}

	[ClientRpc]
	private void RpcSpawnCommand(float x, float z, Command.CommandType type)
	{
		GameObject go = GameObject.Instantiate(CommandPrefab, new Vector3(x, 0.0f, z), Quaternion.identity);

		if(go == null)
		{
			Debug.LogError("failed to instantiate command");
			return;
		}

		Command command = go.GetComponent<Command>();

		if(command == null)
		{
			Debug.LogError("failed to instantiate command");
			return;
		}

		for(int i = 0; i < GameData.CommandIconSpriteInfos.Length; i++)
		{
			if(GameData.CommandIconSpriteInfos[i].Type == type)
			{
				command.Setup(GameData.CommandIconSpriteInfos[i].BackgroundColor, GameData.CommandIconSpriteInfos[i].Icon);
				break;
			}
		}
	}
}
