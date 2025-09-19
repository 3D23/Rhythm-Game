public interface ISceneModel<T> 
    where T : ISceneData
{
    T Data { get; }
}
