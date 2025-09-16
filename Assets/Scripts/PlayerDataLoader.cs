using System.Threading.Tasks;

public class PlayerDataLoader : IGameDataLoader
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
