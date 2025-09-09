using UnityEngine;

[RequireComponent(typeof(RhythmMovement))]
public class Enemy : MonoBehaviour
{
    private RhythmMovement movement;
    [SerializeField] private Metronome metronome;
    private double t = 0d;

    void Start()
    {
        movement = GetComponent<RhythmMovement>();
    }

    private void Update()
    {
        if (t >= metronome.Interval / 2d)
        {
            if (movement)
                movement.SetAcceleration(new Vector2(Random.Range(1 / 3, 5), 1));
            t = 0;
        }
        t += Time.deltaTime;
    }
}