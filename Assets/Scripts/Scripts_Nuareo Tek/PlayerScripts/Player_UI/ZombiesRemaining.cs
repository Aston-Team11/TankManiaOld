using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombiesRemaining : MonoBehaviour
{
    private Text zombiesRemaining;

    private int remaining;
    GameObject spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.Find("Spawners");
        zombiesRemaining = GetComponent<Text>() as Text;
    }

    // Update is called once per frame
    void Update()
    {
        remaining = spawner.GetComponent<Spawner>().getRemaining();
        zombiesRemaining.text = remaining.ToString();
    }
}

