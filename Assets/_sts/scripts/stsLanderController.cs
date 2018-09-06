using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stsLanderController : MonoBehaviour
{
	public Vector2 ThrusterForce = new Vector2(5, 15);
	private Vector3 _ForceVector;
	private Rigidbody _Body;
	private nvpGame _Game;
	private byte _PosUpdateCounter = 0;

	// Use this for initialization
	void Start()
	{
		_Body = this.GetComponent<Rigidbody>();
		_Game = GameObject.Find("sceneManagers").GetComponent<nvpGame>();
	}


	void FixedUpdate()
	{
		_ForceVector.x = Input.GetAxis("Horizontal") * ThrusterForce.x;
		_ForceVector.y = Input.GetAxis("Vertical") * ThrusterForce.y;
		_Body.AddForce(_ForceVector, ForceMode.Force);

		_PosUpdateCounter++;
		if (_PosUpdateCounter % 6 == 0)
		{;
			_Game.SendPosition(this._Body.position);
			_PosUpdateCounter = 0;
		}
	}

}
