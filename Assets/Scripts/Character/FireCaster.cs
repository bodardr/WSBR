using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCaster : MonoBehaviour {    

    public ProjectileAbility fireball; //todo : find a way to change the workaround on line 25 (HERE).

    private Movement movement;

    // Use this for initialization
    void Start ()
    {
		fireball = Instantiate(fireball); //We just copy the settings file.
        movement = GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButton("Fire1") && !fireball.OnCooldown)
            fireball.Use(transform, movement.AimDir);

        fireball.UpdateAbility(transform, movement.AimDir);
    }
}
