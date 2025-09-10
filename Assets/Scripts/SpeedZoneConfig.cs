using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Speed Mode Bpm Config")]
public class SpeedModeBpmConfig : ScriptableObject
{
    [Serializable]
    public class SpeedModeBpmRule
    {
        public SpeedMode Mode;
        public ushort MetronomeBpm;
    }

    public SpeedModeBpmRule[] Rules;

    public ushort GetBpmByMode(SpeedMode mode)
    {
        var rule = Rules.Where((r) => r.Mode == mode).FirstOrDefault();
        if (rule == null)
            return 0;
        return rule.MetronomeBpm;
    }

    public SpeedModeBpmRule GetRule(SpeedMode mode) =>
        Rules.Where((r) => r.Mode == mode).FirstOrDefault();
}
