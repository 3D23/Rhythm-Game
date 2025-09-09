using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public enum RhythmGradation
{
    NONE,
    GOOD,
    OK,
    BAD,
    VERY_BAD
}

public class RhythmController : MonoBehaviour
{
    public Action<RhythmGradation> OnRhythmTick;

    [SerializeField] private Metronome metronome;
    private PlayerInputHandler playerInputHandler;
    private const double NORMAL_DEVIATION = 0.1d;

    private readonly List<double> lastMetronomeTickTimes = new(capacity: 2);
    private double negativeButtonTickTime = -1;
    private double positiveButtonTickTime = -1;

#if DEBUG
    private readonly List<double> diffHistory = new();
#endif

    private void Start()
    {
        playerInputHandler = GetComponent<PlayerInputHandler>();
        if (metronome != null)
            metronome.OnTick += OnTickMetronomeHandler;
        if (playerInputHandler != null)
        {
            playerInputHandler.OnPositiveRhythmSpeedAction += OnPositiveRhythmSpeedActionHandler;
            playerInputHandler.OnNegativeRhythmSpeedAction += OnNegativeRhythmSpeedActionHandler;
        }
    }

    private void OnDestroy()
    {
        if (metronome != null)
            metronome.OnTick -= OnTickMetronomeHandler;
        if (playerInputHandler != null)
        {
            playerInputHandler.OnPositiveRhythmSpeedAction -= OnPositiveRhythmSpeedActionHandler;
            playerInputHandler.OnNegativeRhythmSpeedAction -= OnNegativeRhythmSpeedActionHandler;
        }

#if DEBUG
        if (diffHistory.Count == 0)
            return;
        var avg = diffHistory.Average();
        double disp = 0d;
        var sumSquares = 0d;
        foreach (var dif in diffHistory)
        {
            double deviation = dif - avg;
            sumSquares += deviation * deviation;
        }
        disp = sumSquares / diffHistory.Count;
        WriteToFile($"AVG: {avg}, Диспресия: {disp}, отклонение: {Math.Sqrt(disp)}");
#endif
    }

    private void OnNegativeRhythmSpeedActionHandler()
    {
        negativeButtonTickTime = AudioSettings.dspTime;
        InvokeRythmGrade();
    }

    private void OnPositiveRhythmSpeedActionHandler()
    {
        positiveButtonTickTime = AudioSettings.dspTime;
        InvokeRythmGrade();
    }

    private void OnTickMetronomeHandler()
    {
        if (lastMetronomeTickTimes.Count == 2) 
        { 
            lastMetronomeTickTimes.RemoveAt(0);
            lastMetronomeTickTimes.Add(AudioSettings.dspTime);
        }
        else
        {
            lastMetronomeTickTimes.Add(AudioSettings.dspTime);
        }

        InvokeRythmGrade();
#if DEBUG
        if (lastMetronomeTickTimes.Count == 2 && positiveButtonTickTime > 0 && negativeButtonTickTime > 0)
        {
            if (Math.Max(negativeButtonTickTime, positiveButtonTickTime) - lastMetronomeTickTimes[0] <= -lastMetronomeTickTimes[0] - lastMetronomeTickTimes[1])
                diffHistory.Add(0);
            else
                diffHistory.Add(
                    Math.Abs(positiveButtonTickTime - negativeButtonTickTime)
                    / Math.Abs(lastMetronomeTickTimes[0] - lastMetronomeTickTimes[1]));
        }
#endif
    }

#if DEBUG
    private void WriteToFile(string message)
    {
        string filePath = Application.persistentDataPath + "/Tick_log_120BPM.txt";
        using StreamWriter writer = new(filePath, true);
        writer.WriteLine(message);
    }
#endif

    private void InvokeRythmGrade()
    {
        if (lastMetronomeTickTimes.Count == 2 && positiveButtonTickTime > 0 && negativeButtonTickTime > 0)
        {
            double diff;
            if (Math.Max(negativeButtonTickTime, positiveButtonTickTime) - lastMetronomeTickTimes[0] <= -metronome.Interval)
                diff = 0;
            else
                diff = Math.Abs(positiveButtonTickTime - negativeButtonTickTime)
                    / Math.Abs(lastMetronomeTickTimes[0] - lastMetronomeTickTimes[1]);
            RhythmGradation rhythmGrade;
            if (diff <= 1 + NORMAL_DEVIATION && diff >= 1 - NORMAL_DEVIATION)
                rhythmGrade = RhythmGradation.GOOD;
            else if ((diff <= 1 + 2 * NORMAL_DEVIATION) && (diff >= 1 - 2 * NORMAL_DEVIATION))
                rhythmGrade = RhythmGradation.OK;
            else if ((diff <= 1 + 4 * NORMAL_DEVIATION) && (diff >= 1 - 4 * NORMAL_DEVIATION))
                rhythmGrade = RhythmGradation.BAD;
            else
                rhythmGrade = RhythmGradation.VERY_BAD;

            OnRhythmTick?.Invoke(rhythmGrade);
        }
    }
}