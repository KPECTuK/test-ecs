using Leopotam.EcsLite;
using Modules.Model.Component;
using UnityEngine;

namespace Modules.Model.System
{
	public class ESysMoveEverything : IEcsRunSystem, IEcsInitSystem
	{
		private ISharedState _shared;
		private EcsPool<ECompActionMoveCurrent> _poolLocation;
		private EcsPool<ECompActionMoveTarget> _poolTarget;
		private EcsFilter _filter;
		private IServiceTime _service;

		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_poolLocation = world.GetPool<ECompActionMoveCurrent>();
			_poolTarget = world.GetPool<ECompActionMoveTarget>();
			_filter = world.Filter<ECompActionMoveCurrent>().Inc<ECompActionMoveTarget>().End();
			_shared = systems.GetShared<ISharedState>();
			_service = _shared.GetService<IServiceTime>();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var id in _filter)
			{
				ref var compLocation = ref _poolLocation.Get(id);
				ref var compTarget = ref _poolTarget.Get(id);

				var speed = compTarget.movement.Speed;
				var offset = speed * _service.PassDelta;
				var distance = compTarget.movement.Position - compLocation.Movement.Position;

				compLocation.Movement = new DescMovement
				{
					// скорость, передаваемая в компонент позиции, сохраняется
					Speed = compTarget.movement.Speed,
					Position = Vector3.Dot(offset.normalized, distance.normalized) < 0f
						? compTarget.movement.Position
						: compLocation.Movement.Position + offset,
				};
			}
		}
	}
}
