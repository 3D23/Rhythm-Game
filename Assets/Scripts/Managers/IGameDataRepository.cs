using System.Threading.Tasks;

public interface IGameDataRepository<T, T1>
{
    Task Save();
    Task Load();
    T Get();

    void Set(T1 key, object value);
}
