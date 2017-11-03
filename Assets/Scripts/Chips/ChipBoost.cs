using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipBoost : Chip
{
    public float Thrust = 600.0F;
    public AudioClip BoostSound;

    protected override void executeStart()
    {
        Players.Instance.GetLocal().GetComponent<AudioSource>().PlayOneShot(BoostSound);
        Rigidbody2D rigidBody = Players.Instance.GetLocal().GetComponent<Rigidbody2D>();
        rigidBody.AddForce(rigidBody.transform.up * Thrust);
    }
}
