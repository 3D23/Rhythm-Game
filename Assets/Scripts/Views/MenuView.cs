using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using VContainer;

public class MenuView : MonoBehaviour, IView
{
    [SerializeField] UIDocument document;

    private IGameDataSaver gameDataSaver;
    private Button playButton;
    private Button controlButton;
    private Button quitGameButton;

    [Inject]
    public void Initialize(IGameDataSaver gameDataSaver)
    {
        this.gameDataSaver = gameDataSaver;
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    private async void PlayGame()
    {
        await SceneManager.LoadSceneAsync("Game");
    }

    private void ShowControlWindow()
    {
        Debug.Log("Не реализовано");
    }

    private async void QuitGame()
    {
        await gameDataSaver.Save();
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
