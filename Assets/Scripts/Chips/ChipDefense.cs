using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipDefense : Chip
{
    public AudioClip DefenseSound;

    protected override void executeStart()
    {
        Players.Instance.GetLocal().GetComponent<AudioSource>().PlayOneShot(DefenseSound);
        Tank tankScript = Players.Instance.GetLocal();
        tankScript.CmdAddShield();
    }
}
