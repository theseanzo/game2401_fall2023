using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    public int Health { get; protected set; }

    public virtual void OnHit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            OnDie();
        }
    }

    public virtual void OnDie()
    {
        Destroy(this.gameObject);
    }

    protected virtual void Start()
    {
        this.Health = 50;
    }

    // Add this method to allow updating the health from other classes
    public void SetHealth(int newHealth)
    {
        Health = newHealth;
    }

    void Update()
    {
        Debug.Log(Health);
    }

}
