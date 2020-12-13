using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundNumber : MonoBehaviour
{

    private Text roundNumber;
    Spawner spawner;
    private int wave;
    GameObject gameObject;

    // Start is called before the first frame update
    void Start()
    {
        gameObject = GameObject.Find("Spawners");
        //Spawner round = gameObject.GetComponent<Spawner>();
        roundNumber = GetComponent<Text>() as Text;
    }

    // Update is called once per frame
    void Update()
    {
        wave = gameObject.GetComponent<Spawner>().getWave();
        roundNumber.text = wave.ToString();
    }
}
