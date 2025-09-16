public abstract class GameDataManager<T>
{
    protected readonly IGameDataSetter<T> Setter;
    protected readonly IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> GameDataRepository;

    public GameDataManager(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> repository, IGameDataSetter<T> setter)
    {
        Setter = setter;
        GameDataRepository = repository;
    }

    public abstract T GetData();

    public abstract void UpdateData(T value);
}