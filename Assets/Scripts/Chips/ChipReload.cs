using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipReload : Chip
{
    public AudioClip ReloadSound;

    protected override void executeStart()
    {
        Players.Instance.GetLocal().GetComponent<AudioSource>().PlayOneShot(ReloadSound);
        UserShooting userShootingScript = Players.Instance.GetLocal().GetComponent<UserShooting>();
        userShootingScript.AmmoCount = userShootingScript.ClipMaxSize;
    }
}
