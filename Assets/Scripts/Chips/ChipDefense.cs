using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipDefense : Chip
{
    public AudioClip DefenseSound;

    protected override void executeStart()
    {
        _audioSource.PlayOneShot(DefenseSound);
        Tank tankScript = Players.Instance.GetLocal();
        tankScript.CmdAddShield();
    }
}
