using System;
using UniRx;
using UnityEngine;

[Serializable]
public class RaceSceneData : ISceneData
{
    public ReactiveProperty<float> Energy
    {
        get => energy;
        set => energy.Value = Math.Clamp(value.Value, 0, MaxEnergy.Value);
    }
    public ReactiveProperty<float> MaxEnergy { get; set; } = new(100f);
    public ReactiveProperty<float> EnergyThreshold { get; set; } = new(20f);
    public ReactiveProperty<Vector2> Speed
    {
        get => speed;
        set => speed.Value = new Vector2(Mathf.Clamp(value.Value.x, 0, MaxSpeed.Value), value.Value.y);
    }
    public ReactiveProperty<float> MaxSpeed { get; set; } = new(1.1f);
    public ReactiveProperty<Vector2> Acceleration
    {
        get => acceleration;
        set => acceleration.Value = 
            new Vector2(
                Mathf.Clamp(value.Value.x, -MaxAcceleration.Value, MaxAcceleration.Value), 
                Mathf.Clamp(value.Value.y, -MaxAcceleration.Value, MaxAcceleration.Value)
            );
    }
    public ReactiveProperty<float> MaxAcceleration { get; set; } = new(20f);
    public ReactiveProperty<SpeedMode> SpeedMode { get; set; } = new();
    public ReactiveProperty<ulong> Bmp { get; set; } = new();
    public ReactiveProperty<RaceStatus> RaceStatus { get; set; } = new();

    private readonly ReactiveProperty<float> energy = new();
    private readonly ReactiveProperty<Vector2> acceleration = new();
    private readonly ReactiveProperty<Vector2> speed = new();
}
