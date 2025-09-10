using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Speed Mode Acceleration Config")]
public class SpeedModeAccelerationConfig : ScriptableObject
{
    [Serializable]
    public class SpeedModeAccelerationRule
    {
        public SpeedMode Mode;
        public float Acceleration;
        public float MaxSpeed;
    }

    public SpeedModeAccelerationRule[] Rules;

    public float GetAcceleration(SpeedMode mode)
    {
        var rule = Rules.Where((r) => r.Mode == mode).FirstOrDefault();
        if (rule == null)
            return 0;
        return rule.Acceleration;
    }

    public SpeedModeAccelerationRule GetRule(SpeedMode mode) =>
        Rules.Where((r) => r.Mode == mode).FirstOrDefault();

    public float GetMaxSpeed(SpeedMode mode)
    {
        var rule = Rules.Where((r) => r.Mode == mode).FirstOrDefault();
        if (rule == null)
            return 0;
        return rule.MaxSpeed;
    }
}