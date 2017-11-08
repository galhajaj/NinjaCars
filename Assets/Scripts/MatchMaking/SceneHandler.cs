using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        MyLobby lobby = GameObject.Find("LobbyMgr").GetComponent<MyLobby>();
        lobby.GuiOnPlayScene(false);
        GameObject.Find("LoadingText").GetComponent<WaitingForOpponent>().StopWaiting();

        lobby.DestroyMatch();
        Debug.Log("@@StopClient");
        if (lobby.matchInfo!= null)
        {
            Debug.Log("$$StopHost sceneHandler");
            lobby.StopHost();
        }
        Debug.Log("@@StopMM");
        lobby.StopMatchMaker();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
