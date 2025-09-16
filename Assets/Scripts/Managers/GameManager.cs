using VContainer;

public abstract class GameManager<T>
{
    protected readonly ISetter<PlayerData.PlayerDataFields, T> Setter;
    protected readonly IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> GameDataRepository;

    [Inject]
    public GameManager(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> repository, ISetter<PlayerData.PlayerDataFields, T> setter)
    {
        Setter = setter;
        GameDataRepository = repository;
    }

    public abstract T GetData();

    public abstract void UpdateData(T value);
}