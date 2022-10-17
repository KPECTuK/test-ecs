using System;
using Leopotam.EcsLite;
using Modules.Model;
using Modules.Model.System;
using Service;
using UComponent;
using UnityEngine;

public class Entry : MonoBehaviour, IContext
{
	private IService[] _services;
	private EcsWorld _world;
	private IEcsSystems _systems;

	public ISharedState Shared { get; private set; }

	private ISharedState BuildShared()
	{
		return new SharedStateEngine(this);
	}

	private void Awake()
	{
		if(Application.IsPlaying(this))
		{
			_services = new IService[]
			{
				new ServiceTimeEngine(),
			};

			Shared = BuildShared();
			_world = new EcsWorld();
			_systems = new EcsSystems(_world, Shared);

			_systems
				.Add(new ESysPawnChangeDestination())
				.Add(new ESysButtonCollisions())
				.Add(new ESysCleanUpReached())
				.Add(new ESysMoveEverything())
				.Init();

			var pawn = GetComponentInChildren<CompPawn>() ??
				throw new Exception("cant find pawn asset");
			_world.BuildPawn(
				pawn.transform.position,
				3f,
				.5f,
				out var id);
			pawn.IdEntity = id;

			// =)
			var camera = GetComponentInChildren<CompCamera>() ??
				throw new Exception("cant find camera asset");
			camera.IdEntityPawn = id;

			var pairs = GetComponentsInChildren<CompPair>() ??
				throw new Exception("cant find door pair asset");

			for(var index = 0; index < pairs.Length; index++)
			{
				var compPair = pairs[index];
				var compDoor = compPair.GetComponentInChildren<CompDoor>() ??
					throw new Exception("cant find door asset");

				_world.BuildPair(
					compPair.transform.position,
					.5f,
					compDoor.transform.position,
					1f,
					out var idButton,
					out var idDoor);

				compDoor.IdEntity = idDoor;
				compPair.IdEntity = idButton;
			}
		}
	}

	public T GetService<T>() where T : class, IService
	{
		T result = null;
		for(var index = 0; index < _services.Length; index++)
		{
			result = _services[index] as T;
			//! operator overload bug (Mono)
			if(result != null)
			{
				break;
			}
		}

		return result ?? throw new Exception($"no service found as {typeof(T).Name}");
	}

	public T GetSComponent<T>(int idEntity) where T : struct
	{
		return _world.GetPool<T>().Get(idEntity);
	}

	private void Update()
	{
		for(var index = 0; index < _services.Length; index++)
		{
			// возможно отсюда будет расти ecs движка,
			// однако пользы сейчас в этом не много, потому что компоненты получают апдейты
			// в известной последовательности и с точки зрения алгоритма, нужно уделить внимание
			// только вызову апдейта систем,
			// который здесь происходит в последнюю очередь, рассчитывая следующий апдейт
			_services[index].Update();
		}

		_systems?.Run();
	}

	private void OnDestroy()
	{
		_systems?.Destroy();
		_systems = null;
		_world?.Destroy();
		_world = null;
	}
}
