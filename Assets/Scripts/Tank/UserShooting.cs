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
    public AudioClip ShootSound;
    private AudioSource _audioSource;

    void Start ()
    {
        _audioSource = this.GetComponent<AudioSource>();
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

                CmdThrowShoorikan(this.transform.position, hit.point);
            }
        }
	}

    [Command]
    private void CmdThrowShoorikan(Vector2 origin, Vector2 destination)
    {
        RpcThrowShoorikanOnClient(origin, destination, Random.Range(0.0F, 360.0F));
    }

    [Command]
    private void CmdDestroyPlayer(GameObject player)
    {
        NetworkServer.Destroy(player);
    }

    [ClientRpc]
    public void RpcThrowShoorikanOnClient(Vector2 origin, Vector2 destination, float shoorikanAngle)
    {
        _audioSource.PlayOneShot(ShootSound);

        GameObject shoorikan = Instantiate(ShoorikanObj, destination, Quaternion.identity) as GameObject;
        shoorikan.transform.Rotate(Vector3.forward * shoorikanAngle, Space.World);

        GameObject laser = Instantiate(LaserObj, origin, Quaternion.identity) as GameObject;
        LineRenderer line = laser.GetComponent<LineRenderer>();
        line.SetPosition(0, origin);
        line.SetPosition(1, destination);
        Destroy(line, LaserDuration);
    }
}
