using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject explosion;
    public GameObject player;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("givePowerUp");
            Explode();
        }

    }


    private void Explode()
    {
        var Exploded = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(Exploded, 2f);
        Destroy(this.gameObject);
    }

    private void activatePowerUp(string playerID)
    {
        double result = Random.Range(1, 3);

   switch (result)
   {
       case 1:
           player.SendMessage("PowerupAttained", "SLOMO" + playerID);
             Debug.Log("SENT SLOMO");
           break;
       case 2:
           player.SendMessage("PowerupAttained", "SHIELD" + playerID);
             Debug.Log("SENT SHIELD");
             break;
       default:
           print("ErROR No PoWEr uP FouNd");
           break;
   }
  
      

    }

}
