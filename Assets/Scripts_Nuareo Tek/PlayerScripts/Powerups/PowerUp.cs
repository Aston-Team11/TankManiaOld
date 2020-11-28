using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PowerUp : MonoBehaviourPunCallbacks
{
    public GameObject explosion;
   


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
        //Destroy(this.gameObject);
        //PhotonNetwork.Destroy(this.gameObject);
        photonView.RPC("sendDestroy", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void sendDestroy()
    {
        Destroy(this.gameObject);
    }


    public void activatePowerUp(GameObject player)
    {
     
        double result = Random.Range(1, 3);

   switch (result)
   {
       case 1:
                player.SendMessage("PowerupAttained", "SLOMO");
                Debug.Log("SENT SLOMO");
           break;

       case 2:
                player.SendMessage("PowerupAttained", "SHIELD");
                Debug.Log("SENT SHIELD");
             break;
       default:
           print("ErROR No PoWEr uP FouNd");
           break;
   }

        player.SendMessage("PowerupAttained", "SHIELD");
    }

}
