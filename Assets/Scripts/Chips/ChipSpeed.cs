using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSpeed : Chip
{
    protected override void executeStart()
    {
        LocalPlayer.Instance.Get().MovementData.Thrust *= 2;
    }

    protected override void executeEnd()
    {
        LocalPlayer.Instance.Get().MovementData.Thrust /= 2;
    }
}
