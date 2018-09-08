using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stsLanderController : MonoBehaviour
{
	// +++ public fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public Vector2 ThrusterForce = new Vector2(5, 15);




	// +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	private Vector3 _ForceVector;
	private Rigidbody _Body;
	private nvpMultiplayerManager _MultiplayerManager;
	private byte _PosUpdateCounter = 0;




	// +++ unity life cycle +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start()
	{
		Init();
	}


	void FixedUpdate()
	{
		_ForceVector.x = Input.GetAxis("Horizontal") * ThrusterForce.x;
		_ForceVector.y = Input.GetAxis("Vertical") * ThrusterForce.y;
		_Body.AddForce(_ForceVector, ForceMode.Force);

		_PosUpdateCounter++;
		if (_PosUpdateCounter % 6 == 0)
		{
			_MultiplayerManager.SendPosition(this._Body.position);
			_PosUpdateCounter = 0;
		}
	}




	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	private void Init(){
		_Body = this.GetComponent<Rigidbody>();
		_MultiplayerManager = GameObject.Find("sceneManagers").GetComponent<nvpMultiplayerManager>();
	}
}
