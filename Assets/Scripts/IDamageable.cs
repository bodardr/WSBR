using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    /// <summary>
    /// Damages the entity for an amount
    /// </summary>
    void TakeDamage(int amount, Transform origin);

    /// <summary>
    /// Kills the entity
    /// </summary>
    void Kill();
}
