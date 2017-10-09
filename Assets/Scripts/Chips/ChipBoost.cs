using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipBoost : Chip
{
    public float Thrust = 600.0F;

    protected override void executeStart()
    {
        Rigidbody2D rigidBody = Players.Instance.GetLocal().GetComponent<Rigidbody2D>();
        rigidBody.AddForce(rigidBody.transform.up * Thrust);
        //Players.Instance.GetLocal().MovementData.Thrust *= 2;
    }

    protected override void executeEnd()
    {
        //Players.Instance.GetLocal().MovementData.Thrust /= 2;
    }
}
