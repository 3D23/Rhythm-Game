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
    public Action<Zone> OnSwitchZone;
    [SerializeField] private Zone zone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<RhythmMovement>(out _))
            return;
        OnSwitchZone?.Invoke(zone);
    }
}