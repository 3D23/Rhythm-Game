using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class RacePresenter : ScenePresenter<RaceSceneData>
{
    public RacePresenter(IEnumerable<IGameSavingDataManager> managers, ISceneModel<RaceSceneData> model, IRaceView view) : base(managers, model, view) 
    {
        Model.Data.Energy
            .Subscribe(energy => view.SetEnergy(energy, Model.Data.MaxEnergy.Value, Model.Data.EnergyThreshold.Value))
            .AddTo(Disposables);
        Model.Data.Speed
            .Subscribe(speed => view.SetSpeed(speed, Model.Data.MaxSpeed.Value))
            .AddTo(Disposables);
        Model.Data.SpeedMode
            .Subscribe(view.SetSpeedMode)
            .AddTo(Disposables);
    }

    public void FinishGame() {
        try
        {
            (View as IRaceView).ShowEndGameWindow(Model.Data.RaceStatus.Value);
        }
        catch (InvalidCastException)
        {
            Debug.LogWarning("View is not implement IRaceView");
        }
    }
}
