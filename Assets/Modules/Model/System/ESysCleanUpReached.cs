using Leopotam.EcsLite;
using Modules.Model.Component;

namespace Modules.Model.System
{
	public class ESysCleanUpReached : IEcsRunSystem, IEcsInitSystem
	{
		private const float EPSILON_F = .0001f;

		private EcsPool<ECompActionMoveCurrent> _poolLocation;
		private EcsPool<ECompActionMoveTarget> _poolTarget;
		private EcsFilter _filter;

		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_poolLocation = world.GetPool<ECompActionMoveCurrent>();
			_poolTarget = world.GetPool<ECompActionMoveTarget>();
			_filter = world.Filter<ECompActionMoveCurrent>().Inc<ECompActionMoveTarget>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var id in _filter)
			{
				ref var compLocation = ref _poolLocation.Get(id);
				ref var compTarget = ref _poolTarget.Get(id);

				var distance = compTarget.movement.Position - compLocation.Movement.Position;
				var magnitude = distance.magnitude;

				if(magnitude < EPSILON_F)
				{
					"target reached".Log();

					_poolTarget.Del(id);
				}
			}
		}
	}
}
