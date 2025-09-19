using Cysharp.Threading.Tasks;

public class PlayerDataLoader : IGameDataLoader
{
    private readonly IGameSavingDataRepository<PlayerData, PlayerData.PlayerDataFields> _dataRepository;

    public PlayerDataLoader(IGameSavingDataRepository<PlayerData, PlayerData.PlayerDataFields> repository)
    {
        _dataRepository = repository;
    }

    public async UniTask Load()
    {
        await _dataRepository.Load();
    }
}
