using System;
using UnityEngine;

namespace UComponent
{
	public class CompCamera : MonoBehaviour
	{
		private Camera _camera;
		private Entry _context;

		[NonSerialized]
		public int IdEntityPawn;

		private static readonly Plane _ground = new Plane(Vector3.up, 0f);

		private static readonly float _speedCamera = 5f;
		private static readonly Vector3 _offsetLeft = (Vector3.forward + Vector3.left).normalized * _speedCamera;
		private static readonly Vector3 _offsetRight = (Vector3.back + Vector3.right).normalized * _speedCamera;
		private static readonly Vector3 _offsetUp = (Vector3.forward + Vector3.right).normalized * _speedCamera;
		private static readonly Vector3 _offsetDown = (Vector3.back + Vector3.left).normalized * _speedCamera;

		private void Awake()
		{
			_camera = GetComponent<Camera>();
			_context = GetComponentInParent<Entry>();
		}

		private void Update()
		{
			if(Input.anyKey)
			{
				var offset = Vector3.zero;
				offset += Input.GetKey(KeyCode.DownArrow) ? _offsetDown : Vector3.zero;
				offset += Input.GetKey(KeyCode.UpArrow) ? _offsetUp : Vector3.zero;
				offset += Input.GetKey(KeyCode.LeftArrow) ? _offsetLeft : Vector3.zero;
				offset += Input.GetKey(KeyCode.RightArrow) ? _offsetRight : Vector3.zero;
				transform.position += offset * Time.deltaTime;
			}

			if(Input.mousePresent && Input.GetMouseButtonUp(0))
			{
				var ray = _camera.ScreenPointToRay(Input.mousePosition);
				if(_ground.Raycast(ray, out var enter))
				{
					var point = ray.origin + ray.direction * enter;
					point.DebugDrawCross(Quaternion.identity, Color.yellow, .5f, 1f);
					_context.Shared.SetDestination(IdEntityPawn, point);
				}
			}
		}
	}
}
