using System.Threading.Tasks;

public class PlayerDataLoader : ILoader
{
    private readonly IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> _dataRepository;

    public PlayerDataLoader(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> repository)
    {
        _dataRepository = repository;
    }

    public async Task Load()
    {
        await _dataRepository.Load();
    }
}

public class PlayerDataSaver : ISaver
{
    private readonly IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> _dataRepository;

    public PlayerDataSaver(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> repository)
    {
        _dataRepository = repository;
    }

    public async Task Save()
    {
        await _dataRepository.Save();
    }
}