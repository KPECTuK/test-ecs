namespace Modules.Model {
	public interface IContext
	{
		T GetService<T>() where T : class, IService;
		T GetSComponent<T>(int idEntity) where T : struct;
	}
}
