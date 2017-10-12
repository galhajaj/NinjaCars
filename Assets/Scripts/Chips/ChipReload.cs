using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipReload : Chip
{
    protected override void executeStart()
    {
        UserShooting userShootingScript = Players.Instance.GetLocal().GetComponent<UserShooting>();
        userShootingScript.AmmoCount = userShootingScript.ClipMaxSize;
    }
}
