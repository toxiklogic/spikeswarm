using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommandSpawner : NetworkBehaviour
{
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

		RpcSpawnCommand(x, z);
	}

	[ClientRpc]
	private void RpcSpawnCommand(float x, float z)
	{
		GameObject.Instantiate(CommandPrefab, new Vector3(x, 0.0f, z), Quaternion.identity);
	}
}
