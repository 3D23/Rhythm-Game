using Cysharp.Threading.Tasks;

public interface IGameSavingDataRepository<T, T1>
{
    UniTask Save();
    UniTask Load();
    T Get();
    void Set(T1 key, object value);
}
