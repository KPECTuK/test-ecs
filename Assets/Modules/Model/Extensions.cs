using Leopotam.EcsLite;
using Modules.Model.Component;
using UnityEngine;

namespace Modules.Model
{
	public static class Extensions
	{
		public static void Log(this string message)
		{
			Debug.Log(message ?? "[null]");
		}

		public static void BuildPair(
			this EcsWorld world,
			Vector3 positionInitialButton,
			float radiusEffectiveButton,
			Vector3 positionInitialDoor,
			float speedDoor,
			out int idButton,
			out int idDoor)
		{
			var poolButton = world.GetPool<ECompModelButton>();
			var poolDoor = world.GetPool<ECompModelDoor>();
			var poolLocation = world.GetPool<ECompActionMoveCurrent>();
			var poolCollision = world.GetPool<ECompActionCollided>();

			// create door entity

			idDoor = world.NewEntity();

			{
				ref var component = ref poolDoor.Add(idDoor);
			}

			{
				ref var component = ref poolLocation.Add(idDoor);
				component.Movement = new DescMovement
				{
					Position = positionInitialDoor,
					Speed = Vector3.up * speedDoor,
				};
			}

			// create button entity

			idButton = world.NewEntity();

			{
				ref var component = ref poolButton.Add(idButton);
				component.IdEntityCorresponding = idDoor;
			}

			{
				ref var component = ref poolLocation.Add(idButton);
				component.Movement = new DescMovement
				{
					Position = positionInitialButton,
					Speed = Vector3.zero,
				};
			}

			{
				ref var component = ref poolCollision.Add(idButton);
				component.Radius = radiusEffectiveButton;
			}
		}

		public static void BuildPawn(
			this EcsWorld world,
			Vector3 positionInitial,
			float speedMovement,
			float radiusEffective,
			out int id)
		{
			id = world.NewEntity();

			{
				ref var component = ref world.GetPool<ECompModelPawn>().Add(id);
			}

			{
				ref var component = ref world.GetPool<ECompActionMoveCurrent>().Add(id);
				component.Movement = new DescMovement
				{
					Position = positionInitial,
					Speed = Vector3.forward * speedMovement,
				};
			}

			{
				ref var component = ref world.GetPool<ECompActionCollided>().Add(id);
				component.Radius = radiusEffective;
			}
		}
	}
}
