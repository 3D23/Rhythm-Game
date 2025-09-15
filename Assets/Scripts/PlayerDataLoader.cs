public class PlayerDataLoader : ILoader
{
    private readonly IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> _dataRepository;

    public PlayerDataLoader(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> repository)
    {
        _dataRepository = repository;
    }

    public void Load()
    {
        _dataRepository.Load();
    }
}
