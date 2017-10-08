using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipDefense : Chip
{
    protected override void executeStart()
    {
        Tank tankScript = Players.Instance.GetLocal();
        tankScript.CmdAddShield();
    }
}
