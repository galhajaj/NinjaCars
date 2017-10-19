using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JadeGenerator : NetworkBehaviour
{
    public GameObject JadeObj;
    public float TimeIntervalToCreate = 10.0F;
    private float _timeToCreate;
    public float MinX = -40.0F;
    public float MaxX = 20.0F;
    public float MinY = -40.0F;
    public float MaxY = 20.0F;
    public int NumberOfJadesAllowedToExist = 10;

	void Start ()
    {
        _timeToCreate = TimeIntervalToCreate;
    }
	
	void Update ()
    {
        if (GameObject.FindGameObjectsWithTag("JadeTag").Length >= NumberOfJadesAllowedToExist)
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
        bool getLegalPsitionToPlaceJade = false;
        float randomX = 0.0F;
        float randomY = 0.0F;

        while (!getLegalPsitionToPlaceJade)
        {
            randomX = Random.Range(MinX, MaxX);
            randomY = Random.Range(MinY, MaxY);

            RaycastHit2D hit = Physics2D.Raycast(new Vector3(randomX, randomY), Vector2.zero);
            if (hit.collider == null)
            {
                getLegalPsitionToPlaceJade = true;
            }
            /*else
            {
                Debug.Log("Can't generate jade in " + randomX.ToString() + "," + randomY.ToString() + "... try again");
            }*/
        }


        GameObject jade = Instantiate(JadeObj, new Vector3(randomX, randomY), Quaternion.identity) as GameObject;
        NetworkServer.Spawn(jade);
    }
}
