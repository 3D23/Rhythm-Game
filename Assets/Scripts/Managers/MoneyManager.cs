public class MoneyManager : GameDataManager<int>
{
    public MoneyManager(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> gameDataRepository, MoneySetter moneySetter) 
        : base(gameDataRepository, moneySetter) { }

    public override int GetData() 
        => GameDataRepository.Get().Money;

    public override void UpdateData(int value)
    {
        Setter.Set(value);
    }
}
