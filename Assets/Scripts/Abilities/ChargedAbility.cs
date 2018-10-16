using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedAbility : Ability {

    /// <summary>
    /// Accessor that indicates if the ability can be casted
    /// </summary>
    public bool CanCast
    {
        get
        {
            return currentCooldown <= 0.0f && !Casting;
        }
    }

    //CASTING
    /// <summary>
    /// Indicates if the ability is Winding Up
    /// </summary>
    protected bool casting = false;

    /// <summary>
    /// Accessor for Winding Up
    /// </summary>
    public bool Casting
    {
        get
        {
            return casting;
        }

        private set
        {
            if (!value)
            {
                castingDuration = 0.0f;
            }

            casting = value;
        }
    }

    /// <summary>
    /// The amount of time spent Winding Up
    /// </summary>
    protected float castingDuration;

    /// <summary>
    /// Accessor for windingUpDuration
    /// </summary>
    public float CastingDuration
    {
        get
        {
            return Mathf.Min(castingDuration, MaximumCastTime);
        }
        set
        {
            castingDuration = Mathf.Min(value, MaximumCastTime);
        }
    }

    /// <summary>
    /// The maximum Winding Up time.
    /// </summary>
    public float MaximumCastTime;

    /// <summary>
    /// Override for UpdateAbility.
    /// </summary>
    /// <param name="origin">the caster</param>
    /// <param name="dir">the direction the caster is aiming.</param>
    public override void UpdateAbility(Transform origin, Vector2 dir)
    {
        if (Casting)
            CastingDuration += Time.smoothDeltaTime;

        base.UpdateAbility(origin, dir);
    }

    /// <summary>
    /// Starts casting up the ability.
    /// </summary>
    public virtual void BeginCasting(Transform origin, Vector2 dir)
    {
        Casting = true;
    }

    /// <summary>
    /// Override for Use
    /// </summary>
    /// <param name="origin">the caster's Transform component</param>
    /// <param name="dir">the direction the caster is aiming.</param>
    public override void Use(Transform origin, Vector2 dir)
    {
        Casting = false;
        base.Use(origin, dir);
    }
}
