using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/Charged")]
public class ChargedProjectileAbility : ChargedAbility {

    /// <summary>
    /// The timeout
    /// </summary>
    public readonly float projectileTimeout;

    /// <summary>
    /// The projectile's Speed
    /// </summary>
    public float ProjectileSpeed;

    /// <summary>
    /// The projectile prefab.
    /// </summary>
    public GameObject ProjectilePrefab;

    /// <summary>
    /// The prefab used when the user is casting.
    /// </summary>
    public GameObject CastingPrefab;

    /// <summary>
    /// The instance of the prefab used when the user is casting.
    /// </summary>
    protected GameObject castingProjectile;

    /// <summary>
    /// The distance from the caster.
    /// </summary>
    public float distanceFromCaster;

    /// <summary>
    /// Launches the projectile to the selected direction
    /// </summary>
    /// <param name="origin">The transform from which the ability is used</param>
    /// <param name="dir">The direction to cast the ability to.</param>
    public override void Use(Transform origin, Vector2 dir)
    {
        if (castingProjectile != null)
        {
            Destroy(castingProjectile);
            castingProjectile = null;
        }
            

        GameObject projectile = Instantiate(ProjectilePrefab, origin.position, 
            Quaternion.Euler(ProjectilePrefab.transform.eulerAngles.x,
                            ProjectilePrefab.transform.eulerAngles.y,
                            Rotations.GetAngleFromTarget(dir)));

        projectile.GetComponent<Projectile>().Initialise(dir, origin, (int)(Damage * castingDuration / MaximumCastTime), ProjectileSpeed);

        base.Use(origin, dir);
    }

    /// <summary>
    /// Updates the current casting ability.
    /// </summary>
    /// <param name="origin">the origin of the update.</param>
    /// <param name="dir">the direction the origin is aiming.</param>
    public override void UpdateAbility(Transform origin, Vector2 dir)
    {
        base.UpdateAbility(origin, dir);

        if (Casting && castingProjectile != null)
        {
            castingProjectile.transform.position = origin.position + (Vector3)dir * distanceFromCaster;

            if (castingProjectile.GetComponent<SpriteRenderer>() != null)
            {
                if (castingProjectile.transform.position.y > origin.position.y)
                    castingProjectile.GetComponent<SpriteRenderer>().sortingOrder = origin.GetComponent<SpriteRenderer>().sortingOrder - 1;
                else
                    castingProjectile.GetComponent<SpriteRenderer>().sortingOrder = origin.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
        }
    }

    public override void BeginCasting(Transform origin, Vector2 dir)
    {
        if (Casting)
            return;

        castingProjectile = Instantiate(CastingPrefab, origin.position + (Vector3)dir * distanceFromCaster, Quaternion.identity);
        base.BeginCasting(origin,dir);
    }
}
