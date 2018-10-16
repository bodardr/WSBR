using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour, IDamageable {

    public int Health { get; private set; }
    public int MaxHealth { get; private set;}

    public int InitialMaxHealth;

    public int Exp { get; private set; }
    public int MaxExp { get; private set; }

    [SerializeField]
    int level;

    public int Level { get { return level; } }

    public int InitialMaxExp;
    public float ExpCurve;

	// Use this for initialization
	void Start ()
    {
        Initialise();
	}

    private void Initialise()
    {
        Exp = 0;
        MaxExp = InitialMaxExp;

        MaxHealth = InitialMaxHealth;
        Health = MaxHealth;
    }

    public void AddExp(int amount)
    {
        Exp += amount;
        while (Exp >= MaxExp)
            LevelUp();
    }

    private void LevelUp()
    {
        level++;
        Exp -= MaxExp;
        MaxExp = (int)((float)MaxExp * ExpCurve);
    }

    public void TakeDamage(int amount, Transform origin)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(this);
    }
}
