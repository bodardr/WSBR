using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rotations
{

    /// <summary>
    /// Gets an angle from an origin point and the target and returns the 
    /// necessary angle on the Z axis.
    /// </summary>
    /// <param name="origin">the origin of the angle</param>
    /// <param name="target">the target to aim to</param>
    /// <returns>returns the z angle value for the rotation</returns>
    public static float GetAngleFromTarget(Vector2 origin, Vector2 target)
    {
        return Mathf.Atan2((target - origin).y, (target - origin).x) * Mathf.Rad2Deg; //Sets rotation to the aiming direction
    }

    /// <summary>
    /// Gets an angle from an origin point and the target and returns the 
    /// necessary angle on the Z axis.
    /// </summary>
    /// <param name="dir">the direction to aim to</param>
    /// <returns>returns the z angle value for the rotation</returns>
    public static float GetAngleFromTarget(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; //Sets rotation to the aiming direction
    }
}
