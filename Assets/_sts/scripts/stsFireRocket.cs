using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets._sts.scripts.messages;

namespace Assets._sts.scripts
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class stsFireRocket : MonoBehaviour, IRemoteMessageHandler
	{
		public bool IsLocalPlayer;

		private nvpGame _Game;
		private Rigidbody _Body;

		void Start()
		{
			_Body = this.GetComponent<Rigidbody>();
			_Game = GameObject.Find("sceneManagers").GetComponent<nvpGame>();
		}

		void Update()
		{
			if (!IsLocalPlayer) return;

			if (Input.GetButtonDown("Fire1"))
			{
				var direction = _Body.velocity;
				CreateRocket(_Body.position, direction);
				_Game.SendRocket(_Body.position, direction);
			}
		}
		
		public void HandleMessage(object msg)
		{
			if (msg is RocketFiredMessage)
			{
				var positionUpdateMessage = (RocketFiredMessage)msg;
				CreateRocket(positionUpdateMessage.ToPosition(), positionUpdateMessage.ToDirection());
			}
		}

		private void CreateRocket(Vector3 position, Vector3 direction)
		{
			Debug.Log($"Fire rocket (Local? {IsLocalPlayer})");
		}
	}

}
