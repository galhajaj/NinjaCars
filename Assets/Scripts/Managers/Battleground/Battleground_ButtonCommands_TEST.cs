using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battleground_ButtonCommands_TEST : MonoBehaviour
{

	void Start ()
    {
        Debug.LogWarning("remember to remove BattlegroundButtonCommands_TEST script & button who activate it");
	}
	
	void Update ()
    {
		
	}

    public void TakeDamage(float damage)
    {
        Tank.Instance.LifeData.Life -= damage;
    }

    public void UsePower(float power)
    {
        Tank.Instance.PowerData.Power -= power;
    }
}
