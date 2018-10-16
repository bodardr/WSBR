using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    /// <summary>
    /// The origin from which the projectile has been launched.
    /// </summary>
    private Transform origin;

    /// <summary>
    /// The projectile's speed.
    /// </summary>
    private float speed;

    /// <summary>
    /// This projectile's damage value.
    /// </summary>
    public int Damage { get; set; }

    public GameObject ImpactPE;

    /// <summary>
    /// The timeout before the projectile is destroyed. 
    /// </summary>
    float projectileTimeout = 3;

    /// <summary>
    /// This is the direction the projectile is launched to.
    /// </summary>
    private Vector2 dir;

    private Rigidbody2D rb = null;

    private Animator anim = null;

    public void Initialise(Vector2 dir, Transform origin, int damage, float speed)
    {
        this.dir = dir;
        this.origin = origin;
        this.speed = speed;
        Damage = damage;
    }

    // Use this for initialization
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Destroy(gameObject, projectileTimeout);
    }

    // Update is called once per frame
    public void Update()
    {
            rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == ("Enemy") && origin.tag != "Enemy")
        {
            collider.gameObject.GetComponent<IDamageable>().TakeDamage(Damage,origin);
            Generate.DamageValue(collider.transform.position, Damage); //todo : implement crits?
            Destroy(gameObject);
            if(ImpactPE!=null)
                Instantiate(ImpactPE, transform.position,
                    Quaternion.Euler(ImpactPE.transform.rotation.eulerAngles.x,
                    ImpactPE.transform.rotation.eulerAngles.y, UnityEngine.Random.Range(-90, 90)));
        }
        else if (collider.tag == "Player" && origin.tag != "Player")
        {
            collider.GetComponent<IDamageable>().TakeDamage(Damage, origin);
            Destroy(gameObject);
            if(ImpactPE!=null)
                Instantiate(ImpactPE, transform.position, 
                Quaternion.Euler(ImpactPE.transform.rotation.eulerAngles.x,
                ImpactPE.transform.rotation.eulerAngles.y, UnityEngine.Random.Range(0,180)));
        }
    }
}
