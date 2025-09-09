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
    [SerializeField] private ZoneController zoneController;
    [SerializeField] private SpeedZoneConfig speedZoneConfig;
    private PlayerInputHandler playerInputHandler;
    private RhythmMovement movement;

    public SpeedMode CurrentMode { get; private set; }

    private void Start()
    {
        playerInputHandler = GetComponent<PlayerInputHandler>();
        movement = GetComponent<RhythmMovement>();
        if (playerInputHandler != null)
        {
            playerInputHandler.OnPositiveSpeedModeAction += PositiveSpeedModeActionHandler;
            playerInputHandler.OnNegativeSpeedModeAction += NegativeSpeedModeActionHandler;
        }
    }

    private void OnDestroy()
    {
        if (playerInputHandler != null)
        {
            playerInputHandler.OnPositiveSpeedModeAction -= PositiveSpeedModeActionHandler;
            playerInputHandler.OnNegativeSpeedModeAction -= NegativeSpeedModeActionHandler;
        }
    }

    private void Update()
    {
        if (movement != null) 
        {
            if (!speedZoneConfig.IsSpeedAllowedInZone(zoneController.CurrentZone, CurrentMode))
            {
                if (movement.enabled)
                {
                    movement.Stop();
                    movement.enabled = false;
                }
            }
            else
            {
                if (!movement.enabled)
                    movement.enabled = true;
            }
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

        if (!speedZoneConfig.IsSpeedAllowedInZone(zoneController.CurrentZone, newMode))
            return false;

        if (newMode != CurrentMode)
        {
            CurrentMode = newMode;
            return true;
        }
        CurrentMode = newMode;
        return false;
    }

    private void NegativeSpeedModeActionHandler() =>
        TryToogleMode(isIncrease: false);

    private void PositiveSpeedModeActionHandler() =>
        TryToogleMode();
}