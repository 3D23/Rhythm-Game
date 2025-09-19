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

    public void SetEnergy(float value) =>
        Model.Data.Energy.Value = value;

    public void SetMaxEnergy(float value) =>
        Model.Data.MaxEnergy.Value = value;

    public void SetSpeed(Vector2 value) =>
        Model.Data.Speed.Value = value;

    public void SetMaxSpeed(float value) =>
        Model.Data.MaxSpeed.Value = value;

    public void SetSpeedMode(SpeedMode mode) =>
        Model.Data.SpeedMode.Value = mode;

    public void SetEnergyThreshold(float value) =>
        Model.Data.EnergyThreshold.Value = value;
}
