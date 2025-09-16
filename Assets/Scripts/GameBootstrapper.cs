using VContainer;
using VContainer.Unity;

public class GameBootstrapper : IStartable
{
    [Inject] private readonly MoneyManager moneyManager;
    [Inject] private readonly PlayerDataLoader playerDataLoader;
    [Inject] private readonly PlayerDataSaver playerDataSaver;

    public async void Start()
    {
        await playerDataLoader.Load();
    }
}
