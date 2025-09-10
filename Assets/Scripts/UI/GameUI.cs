using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private RhythmMovement playerMovement;
    [SerializeField] private ZoneController zoneController;
    [SerializeField] private SpeedModeController speedModeController;

    private VisualElement energyEl;
    private TemplateContainer endGame;
    private Label speed;
    private Button restartButton;
    private VisualElement speedEl;
    private readonly StyleColor baseColor = new(new Color(0.29f, 0.29f, 0.29f));
    private readonly StyleColor baseColor2 = new(new Color(0.345f, 0.03f, 0f));

    private void Start()
    {
        if (document == null)
            return;
        energyEl = document.rootVisualElement.Q<VisualElement>("PlayerEnergy").Q<VisualElement>("energyLevel");
        speed = document.rootVisualElement.Q<Label>("Speed");
        speedEl = document.rootVisualElement.Q<VisualElement>("PlayerSpeed").Q<VisualElement>("energyLevel");
        endGame = document.rootVisualElement.Q<TemplateContainer>("EndGame");
        restartButton = endGame.Q<Button>("RestartButton");
        if (endGame != null)
            endGame.style.visibility = Visibility.Hidden;
        restartButton.clicked += RestartButton;
        zoneController.OnFinishGame += ShowEndGameWindow;
    }

    private void OnDestroy()
    {
        restartButton.clicked -= RestartButton;
        zoneController.OnFinishGame -= ShowEndGameWindow;
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

    private void ShowEndGameWindow()
    {
        if (endGame != null)
            endGame.style.visibility = Visibility.Visible;
    }

    private void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
