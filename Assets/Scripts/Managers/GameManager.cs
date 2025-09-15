using VContainer;

public abstract class GameManager<T>
{
    protected readonly ISaver Saver;
    protected readonly IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> GameDataRepository;

    [Inject]
    public GameManager(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> repository, ISaver saver)
    {
        Saver = saver;
        GameDataRepository = repository;
    }

    public abstract T GetData();

    public virtual void UpdateData(T value)
    {
        Saver.Save(value);
    }
}