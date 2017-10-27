using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUser : MonoBehaviour
{
    public int StartPower = 5;
    public int Power;
    //public float PowerRegenerationRate = 0.02F; // 1/sec

    void Start()
    {
        InitPower();
    }

    public void InitPower()
    {
        Power = StartPower;
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
