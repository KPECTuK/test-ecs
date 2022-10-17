using Modules.Model.Component;
using UnityEngine;

namespace UComponent
{
	public class CompDoor : MonoBehaviour
	{
		public int IdEntity;

		private Entry _context;

		public void Awake()
		{
			_context = GetComponentInParent<Entry>();
		}

		private void Update()
		{
			var component = _context.Shared.GetSComponent<ECompActionMoveCurrent>(IdEntity);
			transform.position = component.Movement.Position;
		}
	}
}
