using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using VContainer;

public class MenuView : MonoBehaviour, IMenuView
{
    [SerializeField] UIDocument document;

    private IGameDataSaver gameDataSaver;
    private MoneyManager moneyManager;
    private Button playButton;
    private Button controlButton;
    private Button quitGameButton;

    [Inject]
    public void Initialize(IGameDataSaver gameDataSaver, MoneyManager moneyManager)
    {
        this.gameDataSaver = gameDataSaver;
        this.moneyManager = moneyManager;
    }

    public async void PlayGame()
    {
        await SceneManager.LoadSceneAsync("Race");
    }

    public void ShowControlWindow()
    {
        Debug.Log("Не реализовано");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Init()
    {
        playButton = document.rootVisualElement.Q<Button>("PlayButton");
        controlButton = document.rootVisualElement.Q<Button>("ControlButton");
        quitGameButton = document.rootVisualElement.Q<Button>("QuitButton");

        playButton.clicked += PlayGame;
        controlButton.clicked += ShowControlWindow;
        quitGameButton.clicked += QuitGame;
    }

    public void Dispose()
    {
        playButton.clicked -= PlayGame;
        controlButton.clicked -= ShowControlWindow;
        quitGameButton.clicked -= QuitGame;
    }
}
