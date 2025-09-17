using System;
using UnityEngine;

public enum Zone
{
    None,
    First,
    Second,
    Third,
    Finish
}

[RequireComponent(typeof(Collider2D))]
public class ZoneLine : MonoBehaviour
{
    public Action<Zone, bool> OnSwitchZone;
    [SerializeField] private Zone zone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out RhythmMovement rhythmComponent))
            return;
        if (rhythmComponent.GetComponent<Enemy>())
        {
            OnSwitchZone?.Invoke(zone, false);
        }
        else if (rhythmComponent.GetComponent<PlayerInputHandler>())
        {
            OnSwitchZone?.Invoke(zone, true);
        }
    }
}