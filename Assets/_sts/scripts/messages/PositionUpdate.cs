using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Nakama.TinyJson;

namespace Assets._sts.scripts.messages
{
	public struct PositionUpdateMessage
	{
		public PositionUpdateMessage(Vector3 position)
		{
			X = position.x;
			Y = position.y;
			Z = position.z;
		}

		public const int OpCode = 100;

		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public Vector3 ToPosition()
		{
			return new Vector3(X, Y, Z);
		}
	}
}
