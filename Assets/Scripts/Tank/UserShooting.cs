using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserShooting : NetworkBehaviour
{
    public GameObject ShoorikanObj;
    public GameObject LaserObj;
    public float LaserDuration = 0.1F;
    public float ShootDistanceFromTank = 0.25F;

    void Start ()
    {

    }
	
	void Update ()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 shootDir = Quaternion.AngleAxis(90.0F, Vector3.forward) * this.transform.right;
            Vector3 shootPosition = this.transform.position + ShootDistanceFromTank * shootDir;

            RaycastHit2D hit = Physics2D.Raycast(shootPosition, shootDir);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
                    CmdDestroyPlayer(hit.collider.gameObject);
                }

                CmdHitWithShoorikan(this.transform.position, hit.point);
            }
        }
	}

    [Command]
    private void CmdHitWithShoorikan(Vector2 origin, Vector2 destination)
    {
        GameObject shoorikan = Instantiate(ShoorikanObj, destination, Quaternion.identity) as GameObject;
        shoorikan.transform.Rotate(Vector3.forward * Random.Range(0.0F, 360.0F), Space.World);
        NetworkServer.Spawn(shoorikan);

        RpcDoOnClient(origin, destination);
    }

    [Command]
    private void CmdDestroyPlayer(GameObject player)
    {
        NetworkServer.Destroy(player);
    }

    [ClientRpc]
    public void RpcDoOnClient(Vector2 origin, Vector2 destination)
    {
        GameObject laser = Instantiate(LaserObj, origin, Quaternion.identity) as GameObject;
        LineRenderer line = laser.GetComponent<LineRenderer>();
        line.SetPosition(0, origin);
        line.SetPosition(1, destination);
        Destroy(line, LaserDuration);
    }
}
