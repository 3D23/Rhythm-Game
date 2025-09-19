public class MoneySetter : IGameSavingDataSetter<int>
{
    private readonly IGameSavingDataRepository<PlayerData, PlayerData.PlayerDataFields> gameDataRepository;

    public MoneySetter(IGameSavingDataRepository<PlayerData, PlayerData.PlayerDataFields> repository)
    {
        gameDataRepository = repository;
    }

    public void Set(int value) =>
        gameDataRepository.Set(PlayerData.PlayerDataFields.Money, value);
}
