using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserShooting : NetworkBehaviour
{
    public int ClipMaxSize = 10;
    public int AmmoCount;
    public float AmmoRegenerationRate = 1.25F;
    private float _timeToAddAmmo;
    public float FireRate = 0.2F;
    private float _timeToShoot;
    public float ShootXDirError = 0.25F;
    public float ShootYDirError = 0.25F;
    public GameObject ShoorikanObj;
    public GameObject LaserObj;
    public float LaserDuration = 0.1F;
    public float ShootDistanceFromTank = 0.25F;
    public AudioClip ShootSound;
    private AudioSource _audioSource;

    void Start ()
    {
        _audioSource = this.GetComponent<AudioSource>();
        AmmoCount = ClipMaxSize;
        _timeToAddAmmo = AmmoRegenerationRate;
        _timeToShoot = 0.0F;
    }
	
	void Update ()
    {
        if (!isLocalPlayer)
            return;

        fireInput();
        ammoRegenration();
    }

    private void fireInput()
    {
        if (_timeToShoot > 0.0F)
            _timeToShoot -= Time.deltaTime;

        if (AmmoCount <= 0)
            return;

        if (Input.GetKey(KeyCode.Space))
        {
            if (_timeToShoot <= 0.0F)
            {
                AmmoCount--;
                _timeToShoot = FireRate;

                FireSingle();
            }
        }
    }

    public void FireSingle()
    {
        Vector3 shootDir = Quaternion.AngleAxis(90.0F, Vector3.forward) * this.transform.right;
        Vector3 errorToShootDir = new Vector3(Random.Range(-ShootXDirError, ShootXDirError), Random.Range(-ShootYDirError, ShootYDirError));
        shootDir += errorToShootDir;
        Vector3 shootPosition = this.transform.position + ShootDistanceFromTank * shootDir;

        // layers to ignore
        int layerMask = (1 << LayerMask.NameToLayer("JadeLayer"));
        //layerMask |= (1 << 13);
        //layerMask |= (1 << 15);
        layerMask = ~layerMask;

        RaycastHit2D hit = Physics2D.Raycast(shootPosition, shootDir, 1000.0F, layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                if (hit.collider.transform.childCount <= 0) // if has no shield - destroy player
                {
                    CmdUpdateScore(isServer);
                    CmdRespawnEnemyAfterDeath(hit.collider.gameObject);
                    //CmdDestroyPlayer(hit.collider.gameObject);
                }
                else // else - remove one shield
                {
                    CmdRemoveShield(hit.collider.gameObject);
                }
            }

            CmdThrowShoorikan(this.transform.position, hit.point);
        }
    }

    [Command]
    public void CmdRemoveShield(GameObject obj)
    {
        RpcRemoveShieldOnClient(obj);
    }

    [ClientRpc]
    private void RpcRemoveShieldOnClient(GameObject obj)
    {
        int shieldCount = obj.transform.childCount;
        if (shieldCount <= 0)
            return;

        Transform shieldToRemove = obj.transform.GetChild(shieldCount - 1);
        shieldToRemove.transform.SetParent(null);
        Destroy(shieldToRemove.gameObject);
    }

    [Command]
    private void CmdRespawnEnemyAfterDeath(GameObject player)
    {
        RpcRespawnEnemyAfterDeath(player);
    }

    [ClientRpc]
    private void RpcRespawnEnemyAfterDeath(GameObject player)
    {
        NetworkStartPosition[] spawnPoints;
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();

        Vector3 spawnPoint = Vector3.zero;
        spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        player.transform.position = spawnPoint;

        // add extra skill
        if (Players.Instance.GetLocal().gameObject == player)
        {
            SkillsBar.Instance.AddUniqueRandomChips(1);
        }
    }

    private void ammoRegenration()
    {
        if (AmmoCount >= ClipMaxSize)
        {
            AmmoCount = ClipMaxSize;
            return;
        }

        _timeToAddAmmo -= Time.deltaTime;
        if (_timeToAddAmmo <= 0.0F)
        {
            _timeToAddAmmo = AmmoRegenerationRate;
            AmmoCount++;
        }
    }

    [Command]
    private void CmdUpdateScore(bool isAddForHost)
    {
        if (isAddForHost)
            MatchParams.Instance.HostPlayerScore++;
        else
            MatchParams.Instance.VisitorPlayerScore++;
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
