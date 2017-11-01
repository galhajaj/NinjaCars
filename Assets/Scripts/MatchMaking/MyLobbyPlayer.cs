using System;
using UnityEngine;
using UnityEngine.Networking;

public class MyLobbyPlayer : NetworkLobbyPlayer
{
   void Awake() {
      DontDestroyOnLoad(transform.gameObject);
   }

   void Start()
   {//added by alon
      if (isLocalPlayer)
      {
         if (!readyToBegin)
         {
            Debug.Log("ReadyToBegin");
            SendReadyToBeginMessage();
         }
         else
         {
            Debug.Log("Not local");
         }
      }
   }
}
