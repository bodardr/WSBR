using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the concept of an Ability that can be stocked
/// as a ScriptableObject (an Asset).
/// </summary>
public abstract class Ability : ScriptableObject
{
    /// <summary>
    /// The damage of the projectile
    /// </summary>
    public int Damage;

    //COOLDOWN
    /// <summary>
    /// The current cooldown amount of the ability.
    /// </summary>
    protected float currentCooldown;

    /// <summary>
    /// Specifies a cooldown for after the ability is casted.
    /// </summary>
    public float Cooldown;

    /// <summary>
    /// Accessor to tell if the ability is on cooldown.
    /// </summary>
    public bool OnCooldown
    {
        get
        {
            return currentCooldown > 0.0f;
        }
    }

    // Update is called once per frame
    public virtual void UpdateAbility(Transform origin, Vector2 dir)
    {
        if (OnCooldown)
            currentCooldown -= Time.smoothDeltaTime;
    }

    /// <summary>
    /// Casts the ability to the desired location.
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="dir"></param>
    public virtual void Use(Transform origin, Vector2 dir)
    {
        currentCooldown = Cooldown;
    }

}
