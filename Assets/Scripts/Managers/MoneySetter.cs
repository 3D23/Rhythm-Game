public class MoneySetter : IGameDataSetter<int>
{
    private readonly IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> gameDataRepository;

    public MoneySetter(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> repository)
    {
        gameDataRepository = repository;
    }

    public void Set(int value) =>
        gameDataRepository.Set(PlayerData.PlayerDataFields.Money, value);
}
