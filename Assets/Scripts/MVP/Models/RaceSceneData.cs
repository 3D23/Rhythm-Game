using System;
using UniRx;
using UnityEngine;

[Serializable]
public class RaceSceneData : ISceneData
{
    public ReactiveProperty<float> Energy { get; set; } = new();
    public ReactiveProperty<float> MaxEnergy { get; set; } = new();
    public ReactiveProperty<float> EnergyThreshold { get; set; } = new();
    public ReactiveProperty<Vector2> Speed { get; set; } = new();
    public ReactiveProperty<float> MaxSpeed { get; set; } = new();
    public ReactiveProperty<SpeedMode> SpeedMode { get; set; } = new();
}
