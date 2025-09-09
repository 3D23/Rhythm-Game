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
        if (zone == Zone.Finish)
        {
            if (!collision.TryGetComponent<PlayerInputHandler>(out _))
                Debug.Log("You Lose");
            else
                Debug.Log("You Win");
        }
        else
        {
            OnSwitchZone?.Invoke(zone);
        }
    }
}