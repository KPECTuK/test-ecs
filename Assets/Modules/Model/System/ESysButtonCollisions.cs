using Leopotam.EcsLite;
using Modules.Model.Component;
using UnityEngine;

namespace Modules.Model.System
{
	public class ESysButtonCollisions : IEcsRunSystem, IEcsInitSystem
	{
		private EcsPool<ECompModelButton> _poolButton;
		private EcsPool<ECompActionCollided> _poolColliding;
		private EcsPool<ECompActionMoveCurrent> _poolLocation;
		private EcsPool<ECompActionMoveTarget> _poolTarget;
		private EcsFilter _filterButtons;
		private EcsFilter _filterColliding;

		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();

			_poolColliding = world.GetPool<ECompActionCollided>();
			_poolLocation = world.GetPool<ECompActionMoveCurrent>();
			_poolTarget = world.GetPool<ECompActionMoveTarget>();
			_poolButton = world.GetPool<ECompModelButton>();
			_filterButtons = world.Filter<ECompActionCollided>().Inc<ECompModelButton>().End();
			_filterColliding = world.Filter<ECompActionCollided>().Exc<ECompModelButton>().End();
		}

		public void Run(IEcsSystems systems)
		{
			// boxing
			foreach(var idButton in _filterButtons)
			{
				ref var compButton = ref _poolButton.Get(idButton);
				ref var compButtonCollided = ref _poolColliding.Get(idButton);
				ref var compButtonLocation = ref _poolLocation.Get(idButton);

				// boxing
				foreach(var idEffector in _filterColliding)
				{
					ref var compEffectorCollided = ref _poolColliding.Get(idEffector);
					ref var compEffectorLocation = ref _poolLocation.Get(idEffector);

					var distance = (compEffectorLocation.Movement.Position - compButtonLocation.Movement.Position).magnitude;
					var threshold = compEffectorCollided.Radius + compButtonCollided.Radius;

					// may exit with no effect
					var idDoor = compButton.IdEntityCorresponding;
					if(distance < threshold)
					{
						if(!_poolTarget.Has(idDoor))
						{
							ref var compDoorLocation = ref _poolLocation.Get(idDoor);
							ref var compTarget = ref _poolTarget.Add(idDoor);

							var ground = compDoorLocation.Movement.Position;
							ground = new Vector3(ground.x, 0f, ground.z);

							compTarget.movement = new DescMovement
							{
								// could be read from component
								Position = ground + Vector3.up * 2f,
								Speed = compDoorLocation.Movement.Speed,
							};

							// можно, не спамить движение а ввести какой нибудь признак открытой двери
							// но это может зависеть от того, как пешка проходит дверь,
							// по этому просто мереть высоту наверное уже не стану
						}
					}
					else
					{
						_poolTarget.Del(idDoor);
					}
				}
			}
		}
	}
}
