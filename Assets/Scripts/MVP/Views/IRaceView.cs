using UnityEngine;

public interface IRaceView : IView
{
    void ShowEndGameWindow(RaceStatus raceStatus);
    void ShowEndGameWindow();
    void Restart();
    void QuitGame();
    void MoveToMenu();
    void SetEnergy(float energy, float maxEnergy, float threshold);
    void SetSpeed(Vector2 speed, float maxSpeed);
    void SetSpeedMode(SpeedMode speedMode);
}
