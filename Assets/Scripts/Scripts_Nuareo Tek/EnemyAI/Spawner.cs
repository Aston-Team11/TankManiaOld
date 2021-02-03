using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private int waveNumber = 0;
    [SerializeField] private int enemySpawnAmount = 0;
    [SerializeField] private int enemiesKilled = 0;


    public GameObject[] spawners;
    public GameObject enemy;
    public GameObject target;
    public int Spawncount = 4;

    //Riyad
    private GameObject player;

    [SerializeField] private List<GameObject> PlayerLists = new List<GameObject>();


    public void IncrementEnemies()
    {
        enemiesKilled++;

        if (photonView.IsMine){
            photonView.RPC("SyncDeathCount", RpcTarget.OthersBuffered, enemiesKilled);
        }

    }


    public int getWave()
    {
        return waveNumber;
    }

    public int getRemaining()
    {
        return enemySpawnAmount - enemiesKilled;
    }

    // Start is called before the first frame update
    void Start()
    {
        //set base vlaues of this class
        waveNumber = 1;
        enemySpawnAmount = 5;
        enemiesKilled = 0;

        spawners = new GameObject[5];

        for(int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }

       
  
    }

    // Update is called once per frame
    void Update()
    {
        if (!(photonView.IsMine)) return;

        if(enemiesKilled >= enemySpawnAmount)
        {
            NextWave();
        }

    }


    // update stats on everyone's screens
    [PunRPC]
     private void SyncWave(int wave)
      {
        waveNumber = wave;
     }

    // update stats on everyone's screens
    [PunRPC]
    private void SyncDeathCount(int killed)
    {
        enemiesKilled = killed;
    }



    private void NextWave()
    {
        waveNumber++;
        enemySpawnAmount += 5;
        enemiesKilled = 0;
        photonView.RPC("SyncWave", RpcTarget.OthersBuffered, waveNumber);

        for (int i = 0; i < enemySpawnAmount; i++)
        {
            SpawnEnemy();        
        }
    }

   
    private void SpawnEnemy()
    {
        Spawncount++;
        int spawnerID = Random.Range(0, spawners.Length);
        

        randomiseSelection();
        var zombie = PhotonNetwork.Instantiate(enemy.name, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);

        zombie.name = "zombie" + Spawncount; // test


    }



    /// <summary>
    /// @author Riyad K Rahman
    /// </summary>
    /// <param name="player"></param>
    public void addPlayer(GameObject player)
    {
        if (!(PlayerLists.Contains(player)))
        {
            PlayerLists.Add(player);
        }

        if (PlayerLists.Count > 1)
        {
            PlayerLists.Sort(sortFunction);
        }

    }


    /// <summary>
    /// @author Riyad K Rahman
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    private int sortFunction(GameObject p1, GameObject p2)
    {
        if (p1.GetComponent<PlayerManager>().GetOrder() < p2.GetComponent<PlayerManager>().GetOrder())
        {
            return -1;
        }
        else if (p1.GetComponent<PlayerManager>().GetOrder() > p2.GetComponent<PlayerManager>().GetOrder())
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }


    /// <summary>
    /// @Riyad K Rahman
    /// Choose a random player to target when spawned
    /// </summary>
    public void randomiseSelection()
    {
        photonView.RPC("targetPlayer", RpcTarget.AllBuffered, Random.Range(0, PlayerLists.Count));
        //targetPlayer(Random.Range(0, PlayerLists.Count));
    }

    [PunRPC]
    private void targetPlayer(int result)
    {
        target = PlayerLists[result];
        enemy.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().getPlayers(target.transform);
    }

}
