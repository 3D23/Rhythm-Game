using System;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(AudioSource))]
public class Metronome : MonoBehaviour
{
    public Action OnTick;
    public double Interval { get; private set; }
    private double nextTickTime;
    private AudioSource audioSource;
    private const ushort INIT_BPM = 120;
    [Inject] private readonly ScenePresenter<RaceSceneData> presenter;
    
    public void SetBpm(ushort value)
    {
        presenter.ModifyModel(model =>
        {
            model.Data.Bmp.Value = value;
        });

        if (value == 0) return;
        Interval = 60.0d / presenter.Model.Data.Bmp.Value;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetBpm(INIT_BPM);
        nextTickTime = AudioSettings.dspTime + Interval;
    }

    private void Update()
    {
        if (AudioSettings.dspTime >= nextTickTime)
        {
            OnTick?.Invoke();
            nextTickTime += Interval;
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}