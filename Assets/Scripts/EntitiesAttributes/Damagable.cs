using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public int MaxLife = 5;
    public float Life = 3;

	void Start ()
    {

    }
	
	void Update ()
    {
        checkDeath();
    }


    private void checkDeath()
    {
        if (Life <= 0.0F)
        {
            Destroy(this.gameObject);
        }
    }
}
