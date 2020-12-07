using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int waveNumber = 0;
    [SerializeField] private int enemySpawnAmount = 0;
    [SerializeField] private int enemiesKilled = 0;



    public GameObject[] spawners;
    public GameObject enemy;
    public GameObject test;



    public void IncrementEnemies()
    {
        enemiesKilled++;
    }


    public int getWave()
    {
        return waveNumber;

    }


    // Start is called before the first frame update
    void Start()
    {
        spawners = new GameObject[5];

        for(int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;


        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesKilled >= enemySpawnAmount)
        {
            NextWave();
        }
        
    }


    private void SpawnEnemy()
    {
        int spawnerID = Random.Range(0, spawners.Length);
        var zombie = Instantiate(enemy, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);
        //zombie.SetActive(true);

       
        test = GameObject.FindGameObjectWithTag("Player");
        zombie.SendMessage("getPlayers", test.transform);
        // need a way to instantiate it for photon

    }

    private void StartWave()
    {
        waveNumber = 1;
        enemySpawnAmount = 5;
        enemiesKilled = 0;

        for(int i = 0;i<enemySpawnAmount;i++)
        {

            SpawnEnemy();
        }
    }


    private void NextWave()
    {
        waveNumber++;
        enemySpawnAmount += 5;
        enemiesKilled = 0;
        for (int i = 0; i < enemySpawnAmount; i++)
        {

            SpawnEnemy();
        }


    }



}
