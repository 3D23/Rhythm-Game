using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Speed Zone Config")]
public class SpeedZoneConfig : ScriptableObject
{
    [Serializable]
    public class ZoneSpeedRule
    {
        public Zone Zone;
        public SpeedMode[] AllowedSpeedModes;
    }

    public ZoneSpeedRule[] Rules;

    public bool IsSpeedAllowedInZone(Zone zone, SpeedMode mode)
    {
        foreach (var rule in Rules)
        {
            if (rule.Zone == zone)
            {
                return Array.Exists(rule.AllowedSpeedModes, s => s == mode);
            }
        }
        return false;
    }
}