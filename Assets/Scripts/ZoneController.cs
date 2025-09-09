using UnityEngine;

public class ZoneController : MonoBehaviour
{
    public Zone CurrentZone {  get; private set; }
    [SerializeField] private ZoneLine[] zonesLine;
    [SerializeField] private Metronome metronome;

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
        if (metronome != null)
            metronome.SetBpm((ushort)Random.Range(30, 290));
    }
}