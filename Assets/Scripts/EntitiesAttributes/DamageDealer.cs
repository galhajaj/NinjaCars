using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float Damage = 0.25F;

    GameObject IgnoreObject = null; // do not damage this object (most of the time because is the shooter)

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        // return ignored object
        if (other.gameObject == IgnoreObject)
            return;

        // return if not contains Damagable script
        Damagable otherDamagableScript = other.GetComponent<Damagable>();
        if (otherDamagableScript == null)
            return;

        //TODO: what happens when it hits an object with no life scale (like walls)
        // decrease life & destroy the ammo
        otherDamagableScript.Life -= this.Damage;
        Destroy(this.gameObject);
    }
}
