public class MoneyManager : GameDataManager<int>
{
    public MoneyManager(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> gameDataRepository, MoneySetter moneySetter) 
        : base(gameDataRepository, moneySetter) { }

    protected override int GetData() 
        => GameDataRepository.Get().Money;

    protected override void UpdateData(int value) =>
        Setter.Set(value);
}
