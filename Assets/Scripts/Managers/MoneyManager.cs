public class MoneyManager : GameManager<int>
{
    public MoneyManager(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> gameDataRepository, MoneySaver moneySaver) 
        : base(gameDataRepository, moneySaver) { }

    public override int GetData() 
        => GameDataRepository.Get().Money;

    public override void UpdateData(int value)
    {
        GameDataRepository.Set(PlayerData.PlayerDataFields.Money, value);
        base.UpdateData(value);
    }
}
