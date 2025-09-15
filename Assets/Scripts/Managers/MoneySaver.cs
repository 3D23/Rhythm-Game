using VContainer;

public class MoneySaver : ISaver
{
    private readonly IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> gameDataRepository;

    [Inject]
    public MoneySaver(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> repository)
    {
        gameDataRepository = repository;
    }

    public void Save(object value)
    {
        gameDataRepository.Save(PlayerData.PlayerDataFields.Money, value);
    }

    public void Load() { }
}
