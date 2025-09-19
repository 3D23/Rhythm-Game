using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RaceView : MonoBehaviour, IRaceView
{
    [SerializeField] private UIDocument document;
    [SerializeField] private PlayerInputHandler inputHandler;

    private VisualElement energyEl;
    private TemplateContainer endGame;
    private Label speed;
    private Button restartButton;
    private Button toMenuButton;
    private Button quitGameButton;
    private VisualElement speedEl;
    private readonly StyleColor baseColor = new(new Color(0.29f, 0.29f, 0.29f));
    private readonly StyleColor baseColor2 = new(new Color(0.345f, 0.03f, 0f));

    public void ShowEndGameWindow(RaceStatus raceStatus)
    {
        if (endGame != null)
            endGame.style.visibility = Visibility.Visible;
    }

    public void ShowEndGameWindow()
    {
        if (endGame != null)
            endGame.style.visibility = Visibility.Visible;
    }

    public async void Restart() =>
        await SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

    public void QuitGame()
    {
        Application.Quit();
    }

    public async void MoveToMenu() =>
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
        restartButton.clicked += Restart;
        toMenuButton.clicked += MoveToMenu;
        quitGameButton.clicked += QuitGame;
        if (inputHandler != null)
            inputHandler.OnCloseButtonClicked += ShowEndGameWindow;
    }

    public void Dispose()
    {
        restartButton.clicked -= Restart;
        toMenuButton.clicked -= MoveToMenu;
        quitGameButton.clicked -= QuitGame;
        if (inputHandler != null)
            inputHandler.OnCloseButtonClicked -= ShowEndGameWindow;
    }

    public void SetEnergy(float energy, float maxEnergy, float threshold)
    {
        energyEl.style.width = Length.Percent((energy / maxEnergy) * 100);
        if (energy < threshold)
            energyEl.style.backgroundColor = baseColor2;
        else
            energyEl.style.backgroundColor = baseColor;
    }

    public void SetSpeed(Vector2 speed, float maxSpeed)
    {
        speedEl.style.width = Length.Percent((speed.x / maxSpeed) * 100);

        if (speed.x >= maxSpeed * 0.9f)
            speedEl.style.backgroundColor = baseColor2;
        else
            speedEl.style.backgroundColor = baseColor;
    }

    public void SetSpeedMode(SpeedMode mode)
    {
        switch (mode)
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
}
