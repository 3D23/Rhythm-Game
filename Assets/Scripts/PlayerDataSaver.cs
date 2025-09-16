using System.Threading.Tasks;

public class PlayerDataSaver : IGameDataSaver
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