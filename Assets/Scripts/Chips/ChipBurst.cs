using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipBurst : Chip
{
    public int AmmoBurstCount = 50;
    public float ErrorX = 10.0F;
    public float ErrorY = 10.0F;

    protected override void executeStart()
    {
        UserShooting userShootingScript = Players.Instance.GetLocal().GetComponent<UserShooting>();

        float originalErrorX = userShootingScript.ShootXDirError;
        float originalErrorY = userShootingScript.ShootYDirError;

        userShootingScript.ShootXDirError = ErrorX;
        userShootingScript.ShootYDirError = ErrorY;

        for (int i = 0; i < AmmoBurstCount; i++)
        {
            userShootingScript.FireSingle();
        }

        userShootingScript.ShootXDirError = originalErrorX;
        userShootingScript.ShootYDirError = originalErrorY;
    }

    protected override void executeEnd()
    {
        
    }
}
