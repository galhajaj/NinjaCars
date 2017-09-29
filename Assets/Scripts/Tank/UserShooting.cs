using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserShooting : NetworkBehaviour
{
    public GameObject ShoorikanObj;
    public float ShootPower = 100.0F;

    void Start ()
    {
        
	}
	
	void Update ()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdShoot();
        }
	}

    [Command]
    private void CmdShoot()
    {
        Vector3 shootDir = Quaternion.AngleAxis(90.0F, Vector3.forward) * this.transform.right;

        GameObject shoorikan = Instantiate(ShoorikanObj, this.transform.position + 1.0F * shootDir, this.transform.rotation) as GameObject;

        shoorikan.GetComponent<Rigidbody2D>().AddForce(shootDir * ShootPower);

        NetworkServer.Spawn(shoorikan);
    }
}
