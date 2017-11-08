using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking.Match;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyLobby : NetworkLobbyManager
{
    protected Prototype.NetworkLobby.LobbyHook _lobbyHooks;//remove
    private GameObject _logonGui;
   private GameObject _playBtn;
    ulong _currentMatchID = 0;
    bool _isServer = false;
    void Start()
   {
        Debug.Log("## MyLobby Start");
        DontDestroyOnLoad(gameObject);
        //MMStart ();
        //      MMListMatches ();

        _logonGui = GameObject.Find("UserDetailsGui");
        _playBtn = GameObject.Find("Gaming");
    }

    public void KickedMessageHandler(NetworkMessage netMsg)
    {
        Debug.Log("## KickedMessageHandler");
        netMsg.conn.Disconnect();
    }

   void MMStart()
   {
        if (IsClientConnected())
        {
            //netMsg.conn.Disconnect();
        }
      Debug.Log ("@ MMStart");
   
        Debug.Log("## Start MatchMaker");
      StartMatchMaker();
   }

   public void MMListMatches()
   {
        Match();


//        MMStart();
      Debug.Log ("@ MMListMatches");

//      matchMaker.ListMatches (0, 20, "", true, 0, 0, OnMatchList);
   }
        
   void MMJoinMatch(MatchInfoSnapshot firstMatch)
   {
      Debug.Log ("@ MMJoinMatch");
      matchMaker.JoinMatch (firstMatch.networkId, "", "", "", 0, 0, OnMatchJoined);
   }

   public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
   {
      Debug.Log ("@ OnMatchJoined");
      base.OnMatchJoined (success, extendedInfo, matchInfo);

      if (!success)
      {
         Debug.Log ("Failed to join match failed: " + extendedInfo);
      }
      else
      {
         Debug.Log("Successfully joined match: " + matchInfo.networkId);
//         if (matches[matchInfo.networkId].currentSize == 2)
//         {
//            
//         }
      }
   }

    void MMCreateMatch(string name)
   {
      Debug.Log ("@ MMCreateMatch");
      matchMaker.CreateMatch(name, 2, true, "", "", "", 0, 0, OnMatchCreate);
      GameObject.Find("LoadingText").GetComponent<WaitingForOpponent>().StartWaiting();
   }

   public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
   {
      Debug.Log("@ OnMatchCreate");
      base.OnMatchCreate(success, extendedInfo, matchInfo);

      if (!success)
      {
         Debug.Log("Failed to join match: " + extendedInfo);
      }
      else
      {
         Debug.Log("Successfully created match: " + matchInfo.networkId);
         _currentMatchID = (System.UInt64)matchInfo.networkId;
         _isServer = true;
      }
   }
   /*
   public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
   {
      bool res = base.OnLobbyServerSceneLoadedForPlayer (lobbyPlayer, gamePlayer);
      GuiOnPlayScene (true);
      Debug.Log ("OnLobbyServerSceneLoadedForPlayer was called");
      return res;
   }
*/
   public override void OnLobbyClientSceneChanged(NetworkConnection conn)
   {
      string temp = SceneManager.GetSceneAt(0).name;
      if (SceneManager.GetSceneAt (0).name == lobbyScene)
      {
         GuiOnPlayScene (false);
      }
      else 
      {
         GuiOnPlayScene(true);
      }
   }
   public void GuiOnPlayScene(bool playScene)
   {
        if (_logonGui == null)
        {
            return;
        }

      GetComponent<Image>().enabled=!playScene;
      _logonGui.SetActive(!playScene);
      _playBtn.SetActive(!playScene);
   }

   public override void OnClientDisconnect(NetworkConnection conn)
   {
      base.OnClientDisconnect(conn);
      GuiOnPlayScene(false);
   }



    //==================================================================================
    //=========================================================================================
    int _currentPage = 0;
    public void Match()
    {
        _currentPage = 0;//get from server last relevant page;
        StartMatchMaker();
        RequestPage(_currentPage);
    }

    public void OnGUIMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (matches.Count == 0)
        {
            string roomName = GameObject.Find("UserDetailsInfo").GetComponent<LogonManager>().GetUsername();
            if (roomName == "")
            {
                roomName = "emptyRoom";
            }

            MMCreateMatch(roomName);
            return;
        }

        for (int i = 0; i < matches.Count; ++i)
        {
            if(matches[0].currentSize == 1) //one player in game
            {
                matchMaker.JoinMatch(matches[i].networkId, "", "", "", 0, 0, OnMatchJoined);
                _currentPage = 0;
                return;
            }
        }

        ++_currentPage;
        RequestPage(_currentPage);
    }

    public void RequestPage(int page)
    {
        matchMaker.ListMatches(_currentPage, 10, "", true, 0, 0, OnGUIMatchList);
        ++_currentPage;
    }

    public void DestroyMatch()
    {
        if (matchInfo == null)
        {
            return;
        }

        Debug.Log("$$StopHost MyLobby");
        StopHost();

//        Debug.Log("## DestoryMatch");
//        if (_isServer == false)
//            return;
//        Debug.Log("## DestroyMatch2");
//        matchMaker.DestroyMatch((NetworkID)_currentMatchID, 0, OnDestroyMatch);
        _currentPage = 0;
        _isServer = false;
    }
}