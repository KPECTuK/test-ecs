using UnityEngine;

namespace Modules.Model
{
	//! pretend to be a service locator
	public interface ISharedState
	{
		bool GetDestination(int idEntity, out Vector3 result);
		void SetDestination(int idEntity, Vector3 value);
		void DropDestination(int idEntity);

		T GetSComponent<T>(int idEntity) where T : struct;

		T GetService<T>() where T : class, IService;
	}
}
