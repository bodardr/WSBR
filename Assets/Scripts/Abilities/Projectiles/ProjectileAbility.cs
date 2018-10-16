using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/Normal")]
public class ProjectileAbility : Ability {

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
    /// The distance from the caster.
    /// </summary>
    public float distancefromCaster;

    public override void Use(Transform origin, Vector2 dir)
    {
        base.Use(origin, dir);

        GameObject projectile = Instantiate(ProjectilePrefab, origin.position + 
            (Vector3)dir * distancefromCaster, Quaternion.Euler(0,0,Rotations.GetAngleFromTarget(dir)));

        projectile.GetComponent<Projectile>().Initialise(dir, origin, (int)(Damage), ProjectileSpeed);
    }
}
