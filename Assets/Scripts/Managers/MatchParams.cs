using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MatchParams : NetworkBehaviour
{
    public static MatchParams Instance = null;

    public int NumberOfRounds = 5;
    private int _numberOfRoundsToWin;
    [SyncVar]
    public int HostPlayerScore = 0;
    [SyncVar]
    public int VisitorPlayerScore = 0;

    void Start ()
    {
        _numberOfRoundsToWin = (NumberOfRounds / 2) + 1;
        Debug.Log("Round to win: " + _numberOfRoundsToWin.ToString());

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
	
	void Update ()
    {
		if (HostPlayerScore >= _numberOfRoundsToWin || VisitorPlayerScore >= _numberOfRoundsToWin)
        {
            Debug.Log("WIN!!!!!!");
            SceneManager.LoadScene("mainScene");
        }
	}
}
