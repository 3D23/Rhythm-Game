public class MoneyManager : GameSavingDataManager<int>
{
    public readonly int Reward = 10;

    public MoneyManager(IGameSavingDataRepository<PlayerData, PlayerData.PlayerDataFields> gameDataRepository, MoneySetter moneySetter) 
        : base(gameDataRepository, moneySetter) { }

    protected override int GetData() 
        => GameDataRepository.Get().Money;

    protected override void UpdateData(int value) =>
        Setter.Set(value);
}
