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
        private static int playerCount;


        /// <summary>
        /// @author Riyad K Rahman
        /// when the game starts spawn players and zombies
        /// </summary>
        // Start is called before the first frame update
        private void Start()
        {
            var target = SpawnPlayer();
            SpawnZombie(target);
        }



        /// <summary>
        /// @author Riyad K Rahman
        /// spawn player, set ID, assign time object so player can control time when powerup is enabled
        /// </summary>
        /// <returns>player transform so that the enemies can find the player </returns>
        private Transform SpawnPlayer()
        {
            playerCount++;
            var player = PhotonNetwork.Instantiate(player_prefab, SpawnPoint.position, SpawnPoint.rotation);
            player.SendMessage("setPlayerID", playerCount);
            player.SendMessage("setTimeObject", time);

            return player.transform;
        }


        //!!!! Create you spawning system here, you can use a IEnumator to wait a certain amount of time before triggering a spawn



        /// <summary>
        /// spawns a zombie and sends the players position
        /// </summary>
        /// <param name="targetPosition"> the new player's poistion in the world</param>
        private void SpawnZombie(Transform targetPosition)
        {
            for (int i = 0; i < 5; i++)
            {
                var zombie = PhotonNetwork.Instantiate(Enemy, SpawnPointEnemy.position, SpawnPointEnemy.rotation);
                zombie.SetActive(true);
                zombie.SendMessage("getPlayers", targetPosition);
            }
        }

    }

}
