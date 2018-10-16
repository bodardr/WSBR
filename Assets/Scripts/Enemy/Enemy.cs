using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable {

    Rigidbody2D rb = null;

    Transform target = null;

    /// <summary>
    /// Represents the attacker (the player to reward with experience).
    /// </summary>
    Transform attacker = null;

    [SerializeField]
    float speed = 0.3f;

    Vector2 dir;

    [SerializeField]
    int health = 0;

    public int maxHealth = 30;

    bool aggro = false;

    public int ExpYield;

    public ChargedProjectileAbility Sting;

    public float AttackingRange;

    // Use this for initialization
    void Start () {
        Init();
        rb = GetComponent<Rigidbody2D>();
	}

    /// <summary>
    /// Initializes the enemy.
    /// </summary>
    private void Init()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(!aggro)
            target = FindCrystal();
        
        float distanceFromTarget = Vector2.Distance(transform.position, target.transform.position);

        Sting.UpdateAbility(transform, dir);

        if(distanceFromTarget < AttackingRange && Sting.CanCast)
            Sting.BeginCasting(transform,dir);

        if(Sting.Casting && Sting.CastingDuration >= Sting.MaximumCastTime)
            Sting.Use(transform,dir);


        if (target != null)
            dir = ((Vector2)target.transform.position - rb.position).normalized;            

        if (dir.magnitude >= 0)
        {
            MoveTowardsDirection();
        }

    }

    /// <summary>
    /// Moves towards the direction (defined as the direction vector).
    /// </summary>
    private void MoveTowardsDirection()
    {
        rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
    }

    private Transform FindCrystal()
    {
        return GameObject.FindGameObjectWithTag("Crystal").transform;
    }

    public void SetAggro(Transform origin)
    {
        aggro = true;
        target = origin;
    }

    /// <summary>
    /// Deals damage to this entity.
    /// </summary>
    /// <param name="amount">the amount of damage taken</param>
    public void TakeDamage(int amount, Transform origin)
    {
        health -= amount;

        this.attacker = origin;

        if (health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        if (attacker.GetComponent<CharacterStats>() != null)
            attacker.GetComponent<CharacterStats>().AddExp(ExpYield);
        Destroy(gameObject);
    }
}
