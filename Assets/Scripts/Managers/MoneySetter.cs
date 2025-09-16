using VContainer;

public class MoneySetter : ISetter<PlayerData.PlayerDataFields, int>
{
    private readonly IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> gameDataRepository;

    [Inject]
    public MoneySetter(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> repository)
    {
        gameDataRepository = repository;
    }

    public void Set(PlayerData.PlayerDataFields key, int value) =>
        gameDataRepository.Set(key, value);
}

public interface ISetter<TKey, TValue>
{
    void Set(TKey key, TValue value);
}