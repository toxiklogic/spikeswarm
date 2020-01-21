using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    private float _originalY;

	void Start ()
	{
        _originalY = transform.position.y;
    }
	
	void Update ()
	{
		// TODO: setup pooled updates
		if (!isLocalPlayer)
			return;

		var x = Input.GetAxis("Horizontal") * 0.1f;
		var z = Input.GetAxis("Vertical") * 0.1f;

		transform.Translate(x, _originalY, z);
	}

	public override void OnStartLocalPlayer()
	{
		GetComponentInChildren<MeshRenderer>().material.color = Color.red;
	}
}
