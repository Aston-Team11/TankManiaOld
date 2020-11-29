using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace com.Riyad.TankMania
{
  
    public class Manager : MonoBehaviour
    {
        public string player_prefab;
        public string mouseReticle;
        public string Enemy;
        public Transform SpawnPoint,SpawnPointEnemy;
        public GameObject time;

       // public FollowPlayer cam;
        private static int playerCount;

        // Start is called before the first frame update
        void Start()
        {
            SpawnPlayer();
        }

        //spawn player, set camera and mouse aim
        public void SpawnPlayer()
        {
            playerCount++;
            var player = PhotonNetwork.Instantiate(player_prefab, SpawnPoint.position, SpawnPoint.rotation);
           // var mouse = PhotonNetwork.Instantiate(mouseReticle, SpawnPoint.position, SpawnPoint.rotation);
            //mouse.SetActive(false);
            player.SendMessage("setPlayerID", playerCount);
           // player.SendMessage("setMouse", mouse);
            player.SendMessage("setTimeObject", time);
            
           // var zombie = PhotonNetwork.Instantiate(Enemy, SpawnPointEnemy.position, SpawnPointEnemy.rotation);
            //zombie.SendMessage("getPlayers", player);
        }
    }

}
