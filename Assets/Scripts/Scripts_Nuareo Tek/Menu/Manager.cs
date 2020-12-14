using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.SceneManagement;


public class Manager : MonoBehaviourPunCallbacks
{
    // public string player_prefab;
    public GameObject playerObject;
    public string mouseReticle;
    public string Enemy;
    public Transform SpawnPoint, SpawnPointEnemy;
    public GameObject time;
    public int playerCount = 0;
    [SerializeField] private int RespawnTime;
    public int count;
    [SerializeField] private List<GameObject> PlayerLists = new List<GameObject>();

    private string playerName;

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
        photonView.RPC("IncrementPlayerCount", RpcTarget.AllBuffered);

       
        //if (!(photonView.IsMine))
        //{
        //    // set player counts
        //    if (GameObject.Find("2") == null)
        //    {
        //        playerName = "2";
             
        //    }

        //    else if (GameObject.Find("3") == null)
        //    {
        //        playerName = "3";
        //    }

        //    else if (GameObject.Find("4") == null)
        //    {
        //        playerName = "4";
        //    }

        //}
        //else
        //{
        //    playerName = "1";
        //}
        //return player.transform;
        StartCoroutine(Timer());
        return transform;
    }


    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        
        var player = PhotonNetwork.Instantiate(playerObject.name, SpawnPoint.position, SpawnPoint.rotation);
        GameObject getNick = GameObject.Find("Launcher");
        player.name = getNick.GetComponent<Launcher>().getNick();

        //player.SendMessage("SetOrder",1);
        // player.SendMessage("setPlayerID", playerCount);
        player.SendMessage("setTimeObject", time);
        PlayerLists.Add(player);
        //Dictionary<int, Photon.Realtime.Player> pList = Photon.Pun.PhotonNetwork.CurrentRoom.Players;
        //foreach (KeyValuePair<int, Photon.Realtime.Player> p in pList)
        //{
        //    print(p.Value.NickName);
        //}

        //Debug.LogWarning(PhotonNetwork.PlayerList);


    }

    [PunRPC]
    public void IncrementPlayerCount()
    {
        playerCount++;

    }


    /// <summary>
    /// @author Riyad K Rahman
    /// spawns a zombie and sends the players position
    /// </summary>
    /// <param name="targetPosition"> the player 1 posistion in the world</param>
    private void SpawnZombie(Transform targetPosition)
    {
        if (!(photonView.IsMine)) return;

        StartCoroutine(DelaySpawn(targetPosition));
        // for (int i = 0; i < 5; i++)
        // {
        //     var zombie = PhotonNetwork.Instantiate(Enemy, SpawnPointEnemy.position, SpawnPointEnemy.rotation);
        //     zombie.SetActive(true);
        //     zombie.name = "zombie" + i;
        //     zombie.SendMessage("TargetPlayer1");
        //
        // }
    }

    IEnumerator DelaySpawn(Transform targetPosition)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 5; i++)
        {
            var zombie = PhotonNetwork.Instantiate(Enemy, SpawnPointEnemy.position, SpawnPointEnemy.rotation);
            zombie.SetActive(true);
            zombie.name = "zombie" + i;
            zombie.SendMessage("TargetPlayer1");

        }
    }

    /// <summary>
    /// Needs fixing
    /// </summary>
    /// <param name="deadPlayer"></param>
    private void Respawn(GameObject deadPlayer)
    {
        //deadPlayer.SetActive(false);
        //StartCoroutine(RespawnPlayer(deadPlayer));
        RespawnTime += 10;
        PlayerLists.Remove(deadPlayer);

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="RespawningPlayer"></param>
    /// <returns></returns>
    IEnumerator RespawnPlayer(GameObject RespawningPlayer)
    {
        //!!!! check player number of alives here? if lives are 0 then dont respawn player 
        yield return new WaitForSeconds(RespawnTime);
        RespawningPlayer.GetComponent<PlayerManager>().StatReset();
        RespawningPlayer.SetActive(true);
        RespawningPlayer.transform.position = SpawnPoint.position;
        PlayerLists.Add(RespawningPlayer);
    }

    public void Update()
    {
        if (photonView.IsMine)
        {
            //CheckForEndGame();
            if (PlayerLists.Count <= 0)
            {
                //photonView.RPC("EndGame", RpcTarget.All);
                //EndGame();
            }
        }

    }


    public void CheckForEndGame()
    {
        foreach (GameObject player in PlayerLists)
        {
            if (player.activeSelf == false)
            {
                count++;
            }

        }

        if (count.Equals(playerCount))
        {
            //EndGame();
            photonView.RPC("EndGame", RpcTarget.All);
        }
    }

    [PunRPC]
    public void EndGame()
    {
        SceneManager.LoadScene("EndCredits", LoadSceneMode.Single);

    }

}

// if (!(photonView.IsMine))
// {
//     Master = GameObject.Find("1");
//     player.name = "2";
// }
// else
// {
//     player.name = "1";
// }