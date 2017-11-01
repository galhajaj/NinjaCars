using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipReload : Chip
{
    public AudioClip ReloadSound;

    protected override void executeStart()
    {
        _audioSource.PlayOneShot(ReloadSound);
        UserShooting userShootingScript = Players.Instance.GetLocal().GetComponent<UserShooting>();
        userShootingScript.AmmoCount = userShootingScript.ClipMaxSize;
    }
}
