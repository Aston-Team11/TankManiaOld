using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace com.Riyad.TankMania
{

    /// <summary>
    /// basic match making system max 20 ppl
    /// </summary>
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
        }

        public override void OnConnectedToMaster()
        { 
            Join();
          
            base.OnConnectedToMaster();
        }


        public override void OnJoinedRoom()
        {
            StartGame();

            base.OnJoinedRoom();
        }

        //if the join fails
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            CreateMatch();
            base.OnJoinRandomFailed(returnCode, message);
        }

        public void Connect() 
        {
            PhotonNetwork.GameVersion = "1.0.0";
            PhotonNetwork.ConnectUsingSettings();

        }

        public void Join()
        {
            PhotonNetwork.JoinRandomRoom();
           
        }

        public void StartGame()
        {
            //if there is exactly one player load the scene
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel(1);
            }
        }

        public void CreateMatch()
        {
            PhotonNetwork.CreateRoom("");
        }



    }


}
