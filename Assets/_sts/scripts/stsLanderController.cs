using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets._sts.scripts.messages;

public class stsLanderController : MonoBehaviour, IRemoteMessageHandler
{
	// +++ public fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public Vector2 ThrusterForce = new Vector2(5, 15);
	public bool isLocalPlayer;
    public Vector3 _ActualPosition;




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
		if(isLocalPlayer) MoveLocalPlayer();
		else MoveRemotePlayer();
	}




	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	private void Init(){
		_Body = this.GetComponent<Rigidbody>();
		_MultiplayerManager = GameObject.Find("sceneManagers").GetComponent<nvpMultiplayerManager>();
	}

	private void MoveLocalPlayer(){
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

	private void MoveRemotePlayer(){
		transform.position = Vector3.Lerp(transform.position, _ActualPosition, 0.2f);
	}

	public void RemoveRigidBody(){
		_Body = this.GetComponent<Rigidbody>();
        Destroy(_Body);
	}


    // +++ interface methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public void HandleMessage(object msg)
    {
        if (msg is PositionUpdateMessage)
        {
            var positionUpdateMessage = (PositionUpdateMessage)msg;
            _ActualPosition = positionUpdateMessage.ToPosition();
        }
    }
}
