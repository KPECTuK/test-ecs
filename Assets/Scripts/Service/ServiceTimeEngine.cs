using Modules.Model;
using UnityEngine;

namespace Service
{
	public sealed class ServiceTimeEngine : IServiceTime
	{
		public float PassDelta { get; private set; }

		public void Update()
		{
			PassDelta = Time.deltaTime;
		}
	}
}
