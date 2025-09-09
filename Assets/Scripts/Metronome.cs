using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Metronome : MonoBehaviour
{
    public Action OnTick;
    public ushort Bpm { get; private set; }
    public double Interval { get; private set; }
    private double nextTickTime;
    private AudioSource audioSource;
    private const ushort INIT_BPM = 120;
    
    public void SetBpm(ushort value)
    {
        Bpm = value;
        if (value == 0) return;
        Interval = 60.0d / Bpm;
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