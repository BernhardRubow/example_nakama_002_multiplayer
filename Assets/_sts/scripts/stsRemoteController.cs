using System.Collections;
using System.Collections.Generic;
using Assets._sts.scripts.messages;
using UnityEngine;

public class stsRemoteController : MonoBehaviour, IRemoteMessageHandler
{
	private Rigidbody _Body;
	public Vector3 _ActualPosition;

	// Use this for initialization
	void Start()
	{
		_Body = this.GetComponent<Rigidbody>();
		Destroy(_Body);
	}

	void FixedUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, _ActualPosition, 0.2f);
	}

	public void HandleMessage(object msg)
	{
		if (msg is PositionUpdateMessage)
		{
			var positionUpdateMessage = (PositionUpdateMessage) msg;
			_ActualPosition = positionUpdateMessage.ToPosition();
		}
	}

}
