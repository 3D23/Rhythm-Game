public interface IGameDataRepository<T, T1>
{
    void Save(T1 key, object value);
    void Load();
    T Get();

    void Set(T1 key, object value);
}
