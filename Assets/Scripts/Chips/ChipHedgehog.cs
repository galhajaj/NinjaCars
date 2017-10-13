using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipHedgehog : Chip
{
    public int FragmentsPerAmmoUnit = 5;
    public float ErrorX = 10.0F;
    public float ErrorY = 10.0F;

    protected override void executeStart()
    {
        UserShooting userShootingScript = Players.Instance.GetLocal().GetComponent<UserShooting>();

        float originalErrorX = userShootingScript.ShootXDirError;
        float originalErrorY = userShootingScript.ShootYDirError;

        userShootingScript.ShootXDirError = ErrorX;
        userShootingScript.ShootYDirError = ErrorY;

        int fragments = FragmentsPerAmmoUnit * Players.Instance.GetLocal().AmmoData.AmmoCount;

        for (int i = 0; i < fragments; i++)
        {
            userShootingScript.FireSingle(UserShooting.AmmoType.FRAGMENT);
        }

        userShootingScript.ShootXDirError = originalErrorX;
        userShootingScript.ShootYDirError = originalErrorY;

        Players.Instance.GetLocal().AmmoData.AmmoCount = 0;
    }

    protected override void executeEnd()
    {
        
    }
}
