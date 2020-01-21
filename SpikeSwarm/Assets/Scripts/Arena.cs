using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Arena : NetworkBehaviour
{
	public static Arena Instance;

	public GameObject MechPrefab;
	public Transform[] MechSpawnPoints;

	private int _nextMechSpawnPoint = 0;

	// Use this for initialization
	void Start ()
	{
		Instance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void ServerSpawnMech()
	{
		RpcSpawnMech(_nextMechSpawnPoint++);
	}

	[ClientRpc]
	private void RpcSpawnMech(int spawnIndex)
	{
		GameObject go = Instantiate(MechPrefab, MechSpawnPoints[spawnIndex].position, MechSpawnPoints[spawnIndex].rotation);
		go.transform.position = MechSpawnPoints[spawnIndex].position;
	}
}
