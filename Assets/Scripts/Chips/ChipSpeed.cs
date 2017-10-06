using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSpeed : Chip
{
    protected override void executeStart()
    {
        Players.Instance.GetLocal().MovementData.Thrust *= 2;
    }

    protected override void executeEnd()
    {
        Players.Instance.GetLocal().MovementData.Thrust /= 2;
    }
}
