using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class GameBootstrapper : IStartable
{ 
    [Inject] private readonly PlayerDataLoader playerDataLoader;

    public async void Start()
    {
        await playerDataLoader.Load();
        await SceneManager.LoadSceneAsync("Menu");
    }
}
