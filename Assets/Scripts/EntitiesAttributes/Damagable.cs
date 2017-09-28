using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public int MaxLife = 5;
    public float Life = 3;
    public float LifeRegenerationRate = 0.05F; // 1/sec
    public int CreditsWhenDestroyed = 0;

	void Start ()
    {

    }
	
	void Update ()
    {
        checkDeath();
        lifeRegeneration();
    }


    private void checkDeath()
    {
        if (Life <= 0.0F)
        {
            DataManager.Instance.Saved.Credits += CreditsWhenDestroyed; // add credits
            Destroy(this.gameObject);
        }
    }

    private void lifeRegeneration()
    {
        int maxLifeToRegenerateTo = Mathf.CeilToInt(Life);

        if (Mathf.FloorToInt(Life) < maxLifeToRegenerateTo)
            Life += Time.deltaTime * LifeRegenerationRate;

        if (Life > maxLifeToRegenerateTo)
            Life = maxLifeToRegenerateTo;
    }
}
