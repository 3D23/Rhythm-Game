using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private RhythmMovement playerMovement;

    private VisualElement energyEl;
    private readonly StyleColor baseColor = new(new Color(0.29f, 0.29f, 0.29f));
    private readonly StyleColor baseColor2 = new(new Color(0.345f, 0.03f, 0f));

    private void Start()
    {
        if (document == null)
            return;
        energyEl = document.rootVisualElement.Q<VisualElement>("PlayerEnergy").Q<VisualElement>("energyLevel");
    }

    private void Update()
    {
        energyEl.style.width = Length.Percent((playerMovement.Energy / playerMovement.MaxEnergy) * 100);
        if (playerMovement.Energy < playerMovement.EnergyThreshold)
            energyEl.style.backgroundColor = baseColor2;
        else 
            energyEl.style.backgroundColor = baseColor;
    }
}
