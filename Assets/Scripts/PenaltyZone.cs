using System.Collections.Generic;
using UnityEngine;

public class PenaltyZone : MonoBehaviour
{
    private List<RhythmMovement> movementsObjects;
    private const float PENALTY_COEF = 1.02f;

    private void Start()
    {
        movementsObjects = new();
    }

    private void OnDestroy()
    {
        movementsObjects.Clear();
    }

    private void Update()
    {
        foreach (var movement in movementsObjects)
            movement.SetAcceleration(new Vector2(1 / PENALTY_COEF, 1));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out RhythmMovement movement))
        {
            if (!movementsObjects.Exists((m) => m == movement))
                movementsObjects.Add(movement);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out RhythmMovement movement))
        {
            if (movementsObjects.Exists((m) => m == movement))
                movementsObjects.Remove(movement);
        }
    }
}