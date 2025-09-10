using System;
using UnityEngine;

public enum SpeedMode
{
    First = 0,
    Second = 1,
    Third = 2
}

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(RhythmMovement))]
public class SpeedModeController : MonoBehaviour
{
    public Action<SpeedModeBpmConfig.SpeedModeBpmRule> OnChangeSpeedMode;
    [SerializeField] private RhythmController rhythmController;
    [SerializeField] private SpeedModeBpmConfig speedModeBpmConfig;
    [SerializeField] private SpeedModeAccelerationConfig speedModeAccelerationConfig;
    private PlayerInputHandler playerInputHandler;
    private float acceleration; 
    private float maxSpeed;
    private RhythmMovement rhythmMovement;

    public SpeedMode CurrentMode { get; private set; } = SpeedMode.First;

    private void Start()
    {
        rhythmMovement = GetComponent<RhythmMovement>();
        acceleration = speedModeAccelerationConfig.GetAcceleration(CurrentMode);
        maxSpeed = speedModeAccelerationConfig.GetMaxSpeed(CurrentMode);
        playerInputHandler = GetComponent<PlayerInputHandler>();
        if (playerInputHandler != null)
        {
            playerInputHandler.OnPositiveSpeedModeAction += PositiveSpeedModeActionHandler;
            playerInputHandler.OnNegativeSpeedModeAction += NegativeSpeedModeActionHandler;
        }
        if (rhythmMovement != null)
            rhythmMovement.OnRhythmAccelerationChange += ChangeAcceleration;
    }

    private void OnDestroy()
    {
        if (rhythmMovement != null)
            rhythmMovement.OnRhythmAccelerationChange -= ChangeAcceleration;
        if (playerInputHandler != null)
        {
            playerInputHandler.OnPositiveSpeedModeAction -= PositiveSpeedModeActionHandler;
            playerInputHandler.OnNegativeSpeedModeAction -= NegativeSpeedModeActionHandler;
        }
    }

    private bool TryToogleMode(bool isIncrease = true)
    {
        int maxValue = Enum.GetValues(typeof(SpeedMode)).Length - 1;
        SpeedMode newMode;
        int minValue = 0;
        if (isIncrease)
        {
            int newModeInt = (int)CurrentMode + 1;
            if (newModeInt > maxValue)
                newModeInt = maxValue;
            newMode = (SpeedMode)newModeInt;
        }
        else
        {
            int newModeInt = (int)CurrentMode - 1;
            if (newModeInt < minValue)
                newModeInt = minValue;
            newMode = (SpeedMode)newModeInt;
        }

        CurrentMode = newMode;
        acceleration = speedModeAccelerationConfig.GetAcceleration(CurrentMode);
        maxSpeed = speedModeAccelerationConfig.GetMaxSpeed(CurrentMode);
        rhythmMovement.SetMaxSpeed(maxSpeed);
        var r = speedModeBpmConfig.GetRule(CurrentMode);
        if (r != null)
        {
            rhythmController.SetBpm(r.MetronomeBpm);
            OnChangeSpeedMode?.Invoke(r);
        }
        return true;
    }

    private void ChangeAcceleration() =>
        rhythmMovement.SetAcceleration(new Vector2(acceleration, 1));

    private void NegativeSpeedModeActionHandler() =>
        TryToogleMode(isIncrease: false);

    private void PositiveSpeedModeActionHandler() =>
        TryToogleMode();
}