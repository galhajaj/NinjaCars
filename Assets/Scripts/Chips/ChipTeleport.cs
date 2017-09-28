using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipTeleport : Chip
{
    protected override void executeStart()
    {
        GameObject tankObj = Tank.Instance.gameObject;
        tankObj.transform.position = new Vector3(
            Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
            tankObj.transform.position.z);
    }
}
