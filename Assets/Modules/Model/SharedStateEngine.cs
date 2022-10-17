using System.Collections.Generic;
using UnityEngine;

namespace Modules.Model
{
	public class SharedStateEngine : ISharedState
	{
		private readonly IContext _context;
		private readonly Dictionary<int, Vector3> _targets = new Dictionary<int, Vector3>();

		public SharedStateEngine(IContext context)
		{
			_context = context;
		}

		public bool GetDestination(int idEntity, out Vector3 result)
		{
			return _targets.TryGetValue(idEntity, out result);
		}

		public void DropDestination(int idEntity)
		{
			_targets.Remove(idEntity);
		}

		public void SetDestination(int idEntity, Vector3 value)
		{
			_targets[idEntity] = value;
		}

		public T GetSComponent<T>(int idEntity) where T : struct
		{
			return _context.GetSComponent<T>(idEntity);
		}

		public T GetService<T>() where T : class, IService
		{
			return _context.GetService<T>();
		}
	}
}
