using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Nakama.TinyJson;

namespace Assets._sts.scripts.messages
{
	public struct RocketFiredMessage
	{
		public RocketFiredMessage(Vector3 position, Vector3 direction)
		{
			PosX = position.x;
			PosY = position.y;
			PosZ = position.z;
			DirX = direction.x;
			DirY = direction.y;
			DirZ = direction.z;
		}

		public const int OpCode = 101;

		public float PosX { get; set; }
		public float PosY { get; set; }
		public float PosZ { get; set; }
		public float DirX { get; set; }
		public float DirY { get; set; }
		public float DirZ { get; set; }

		public Vector3 ToPosition()
		{
			return new Vector3(PosX, PosY, PosZ);
		}
		public Vector3 ToDirection()
		{
			return new Vector3(DirX, DirY, DirZ);
		}
	}
}
