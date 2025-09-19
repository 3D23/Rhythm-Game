using System;
using UnityEngine;
using VContainer;

public class ZoneController : MonoBehaviour
{
    public Action OnFinishGame;
    public Zone CurrentZone {  get; private set; }
    [SerializeField] private ZoneLine[] zonesLine;
    [Inject] private readonly MoneyManager moneyManager;
    [Inject] private readonly RacePresenter presenter;

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
        if (CurrentZone == Zone.Finish)
        {
            if (isPlayerSwitched)
            {
                presenter.ModifyModel(model =>
                {
                    model.Data.RaceStatus.Value = RaceStatus.Win;
                });
                moneyManager.Data = new(moneyManager.Data.Value + moneyManager.Reward);
            }
            else
            {
                presenter.ModifyModel(model =>
                {
                    model.Data.RaceStatus.Value = RaceStatus.Lose;
                });
            }
            presenter.FinishGame();
        }
    }
}