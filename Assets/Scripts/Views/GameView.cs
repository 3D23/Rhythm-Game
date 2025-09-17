using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using VContainer;

public class GameView : MonoBehaviour, IView
{
    [SerializeField] private UIDocument document;
    [SerializeField] private RhythmMovement playerMovement;
    [SerializeField] private ZoneController zoneController;
    [SerializeField] private SpeedModeController speedModeController;
    [SerializeField] private PlayerInputHandler inputHandler;
    [Inject] private readonly IGameDataSaver gameDataSaver;

    private VisualElement energyEl;
    private TemplateContainer endGame;
    private Label speed;
    private Button restartButton;
    private Button toMenuButton;
    private Button quitGameButton;
    private VisualElement speedEl;
    private readonly StyleColor baseColor = new(new Color(0.29f, 0.29f, 0.29f));
    private readonly StyleColor baseColor2 = new(new Color(0.345f, 0.03f, 0f));

    #region Unity
    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    private void Update()
    {
        energyEl.style.width = Length.Percent((playerMovement.Energy / playerMovement.MaxEnergy) * 100);
        speedEl.style.width = Length.Percent((playerMovement.Speed.x / playerMovement.MaxSpeed) * 100);
        if (playerMovement.Energy < playerMovement.EnergyThreshold)
            energyEl.style.backgroundColor = baseColor2;
        else 
            energyEl.style.backgroundColor = baseColor;

        if (playerMovement.Speed.x >= playerMovement.MaxSpeed * 0.9f)
            speedEl.style.backgroundColor = baseColor2;
        else
            speedEl.style.backgroundColor = baseColor;
        switch (speedModeController.CurrentMode)
        {
            case SpeedMode.First:
                speed.text = "1";
                break;
            case SpeedMode.Second:
                speed.text = "2";
                break;
            case SpeedMode.Third:
                speed.text = "3";
                break;
        }
    }
    #endregion

    private void ShowEndGameWindow()
    {
        if (endGame != null)
            endGame.style.visibility = Visibility.Visible;
    }

    private async void RestartButton() =>
        await SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

    private async void QuitGame()
    {
        await gameDataSaver.Save();
        Application.Quit();
    }

    private async void MoveToMenu() =>
        await SceneManager.LoadSceneAsync("Menu");

    public void Init()
    {
        if (document == null)
            return;
        energyEl = document.rootVisualElement.Q<VisualElement>("PlayerEnergy").Q<VisualElement>("energyLevel");
        speed = document.rootVisualElement.Q<Label>("Speed");
        speedEl = document.rootVisualElement.Q<VisualElement>("PlayerSpeed").Q<VisualElement>("energyLevel");
        endGame = document.rootVisualElement.Q<TemplateContainer>("EndGame");
        restartButton = endGame.Q<Button>("RestartButton");
        toMenuButton = endGame.Q<Button>("ToMenuButton");
        quitGameButton = endGame.Q<Button>("QuitGameButton");
        if (endGame != null)
            endGame.style.visibility = Visibility.Hidden;
        restartButton.clicked += RestartButton;
        toMenuButton.clicked += MoveToMenu;
        quitGameButton.clicked += QuitGame;
        zoneController.OnFinishGame += ShowEndGameWindow;
        if (inputHandler != null)
            inputHandler.OnCloseButtonClicked += ShowEndGameWindow;
    }

    public void Dispose()
    {
        restartButton.clicked -= RestartButton;
        zoneController.OnFinishGame -= ShowEndGameWindow;
        toMenuButton.clicked -= MoveToMenu;
        quitGameButton.clicked -= QuitGame;
        if (inputHandler != null)
            inputHandler.OnCloseButtonClicked -= ShowEndGameWindow;
    }
}
