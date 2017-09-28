using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip_Skill_Speed : Chip
{
    protected override void executeStart()
    {
        Tank.Instance.MovementData.Thrust *= 2;
    }

    protected override void executeEnd()
    {
        Tank.Instance.MovementData.Thrust /= 2;
    }
}
