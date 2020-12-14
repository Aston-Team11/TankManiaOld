using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radiation : MonoBehaviour
{
    public float sphereRadius;
    [SerializeField] private GameObject player;
   

    public void Start()
    {
          StartCoroutine(endAll());
    }

    IEnumerator endAll()
    {
        yield return new WaitForSeconds(9f);
        Destroy(this.gameObject);

    }


    private void OnTriggerEnter(Collider playerCollider)
    {

        if (playerCollider.gameObject.CompareTag("Player"))
        {
            //Debug.LogWarning("AOE Damage works");

            player = playerCollider.gameObject;
            player.GetComponent<PlayerManager>().Setpoisoned(true);
        }
    }

    private void OnTriggerExit(Collider playerCollider)
    {
        if (playerCollider.gameObject.CompareTag("Player") )
        {
            player = playerCollider.gameObject;
           player.GetComponent<PlayerManager>().Setpoisoned(false);

        }
    }

      
 }
