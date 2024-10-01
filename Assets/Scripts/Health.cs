using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    [HideInInspector]
    public int health;
    public delegate void OnHealthChanged();
    public event OnHealthChanged onHealthChanged;
    protected virtual void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int someDamage)
    {
        health -= someDamage;
        if (onHealthChanged != null)
        {
            onHealthChanged.Invoke();
        }
        if (health <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        Destroy(gameObject);
    }

    public bool AmIAlive()
    {
        return health > 0;
    }
}
