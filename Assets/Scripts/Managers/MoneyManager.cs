public class MoneyManager : GameManager<int>
{
    public MoneyManager(IGameDataRepository<PlayerData, PlayerData.PlayerDataFields> gameDataRepository, MoneySetter moneySetter) 
        : base(gameDataRepository, moneySetter) { }

    public override int GetData() 
        => GameDataRepository.Get().Money;

    public override void UpdateData(int value)
    {
        GameDataRepository.Set(PlayerData.PlayerDataFields.Money, value);
    }
}
