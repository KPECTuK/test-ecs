using Leopotam.EcsLite;
using Modules.Model.Component;

namespace Modules.Model.System
{
	public class ESysPawnChangeDestination : IEcsRunSystem, IEcsInitSystem
	{
		private ISharedState _shared;
		private EcsPool<ECompActionMoveTarget> _poolTarget;
		private EcsPool<ECompActionMoveCurrent> _poolLocation;
		private EcsFilter _filterLocation;

		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_poolTarget = world.GetPool<ECompActionMoveTarget>();
			_poolLocation = world.GetPool<ECompActionMoveCurrent>();
			_filterLocation = world.Filter<ECompActionMoveCurrent>().Inc<ECompModelPawn>().End();
			_shared = systems.GetShared<ISharedState>();
		}

		public void Run(IEcsSystems systems)
		{
			// that is boxing anyway
			foreach(var id in _filterLocation)
			{
				if(_shared.GetDestination(id, out var destination))
				{
					_shared.DropDestination(id);
					ref var compLocation = ref _poolLocation.Get(id);

					var distance = destination - compLocation.Movement.Position;
					var speed = distance.normalized;

					if(_poolTarget.Has(id))
					{
						_poolTarget.Del(id);
					}

					ref var compTarget = ref _poolTarget.Add(id);
					compTarget.movement = new DescMovement
					{
						Position = destination,
						Speed = speed,
					};
				}
			}
		}
	}
}
