﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipTeleport : Chip
{
    public GameObject TeleportMarkObj;
    private bool _isMarkPlaced = false;
    private Vector2 _positionToTeleportTo;
    public int CostAfterTeleportMarkPlacement = 2;
    public Sprite IconAfterCreation;
    public AudioClip TeleportSetSound;
    public AudioClip TeleportMoveSound;

    protected override void executeStart()
    {
        if (!_isMarkPlaced)
        {
            Players.Instance.GetLocal().GetComponent<AudioSource>().PlayOneShot(TeleportSetSound);
            GameObject teleportMark = Instantiate(TeleportMarkObj, Players.Instance.GetLocal().transform.position, Quaternion.identity) as GameObject;
            _positionToTeleportTo = teleportMark.transform.position;
            _isMarkPlaced = true;
            Cost = CostAfterTeleportMarkPlacement;
            updateCostGui();
            setIcon(IconAfterCreation);
        }
        else
        {
            Players.Instance.GetLocal().GetComponent<AudioSource>().PlayOneShot(TeleportMoveSound);
            GameObject tankObj = Players.Instance.GetLocal().gameObject;
            tankObj.transform.position = new Vector3(
                _positionToTeleportTo.x,
                _positionToTeleportTo.y,
                //Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                //Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
                tankObj.transform.position.z);
        }
    }
}
