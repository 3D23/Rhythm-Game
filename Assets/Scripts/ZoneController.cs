using System;
using UnityEngine;
using VContainer;

public enum RaceStatus
{
    Win,
    Lose
}

public class ZoneController : MonoBehaviour
{
    public Action<RaceStatus> OnFinishGame;
    public Zone CurrentZone {  get; private set; }
    [SerializeField] private ZoneLine[] zonesLine;
    [Inject] private readonly MoneyManager moneyManager;

    private void Start()
    {
        foreach (var line in zonesLine)
            line.OnSwitchZone += SwitchZone;
    }

    private void OnDestroy()
    {
        foreach (var line in zonesLine)
            line.OnSwitchZone -= SwitchZone;
    }

    private void SwitchZone(Zone zone, bool isPlayerSwitched)
    {
        CurrentZone = zone;
        Debug.Log(CurrentZone);
        if (CurrentZone == Zone.Finish)
        {
            if (isPlayerSwitched)
            {
                OnFinishGame(RaceStatus.Win);
                moneyManager.Data = new(moneyManager.Data.Value + moneyManager.Reward);
            }
            else
            {
                OnFinishGame(RaceStatus.Lose);
            }
        }
    }
}