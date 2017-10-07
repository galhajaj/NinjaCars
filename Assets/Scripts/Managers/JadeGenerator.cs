using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JadeGenerator : NetworkBehaviour
{
    public GameObject JadeObj;
    public float TimeIntervalToCreate = 10.0F;
    private float _timeToCreate;
    public float SquareAreaToCreateIn = 5.0F;

	void Start ()
    {
        _timeToCreate = TimeIntervalToCreate;
    }
	
	void Update ()
    {
        if (GameObject.FindGameObjectsWithTag("JadeTag").Length > 0)
            return;

        _timeToCreate -= Time.deltaTime;
        if (_timeToCreate <= 0.0F)
        {
            _timeToCreate = TimeIntervalToCreate;
            CmdSpawnJade();
        }
	}

    [Command]
    private void CmdSpawnJade()
    {
        float randomX = Random.Range(-SquareAreaToCreateIn, SquareAreaToCreateIn);
        float randomY = Random.Range(-SquareAreaToCreateIn, SquareAreaToCreateIn);
        GameObject jade = Instantiate(JadeObj, new Vector3(randomX, randomY), Quaternion.identity) as GameObject;
        NetworkServer.Spawn(jade);
    }
}
