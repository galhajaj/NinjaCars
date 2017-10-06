using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUser : MonoBehaviour
{
    public int Power = 5;
    //public float PowerRegenerationRate = 0.02F; // 1/sec

    void Start()
    {

    }

    void Update()
    {
        //powerRegeneration();
    }

    /*private void powerRegeneration()
    {
        if (Power < MaxPower)
            Power += Time.deltaTime * PowerRegenerationRate;
        else
            Power = MaxPower;
    }*/
}
