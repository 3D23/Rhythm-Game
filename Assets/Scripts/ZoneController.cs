using System;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    public Action OnFinishGame;
    public Zone CurrentZone {  get; private set; }
    [SerializeField] private ZoneLine[] zonesLine;

    private void Start()
    {
        foreach (var line in zonesLine)
            line.OnSwitchZone += SwitchZone;
    }

    private void OnDestroy()
    {
        foreach (var line in zonesLine)
            line.OnSwitchZone -= SwitchZone;
    }

    private void SwitchZone(Zone zone)
    {
        CurrentZone = zone;
        Debug.Log(CurrentZone);
        if (CurrentZone == Zone.Finish)
            OnFinishGame?.Invoke();
    }
}