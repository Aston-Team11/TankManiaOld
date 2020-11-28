using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks
{

    public int health = 100;
    public GameObject explosion;
    private Slo_Motion slow;
    public GameObject shield;
    public Shield shieldClass;

 
    private GameObject mouseTarget;

    //public bounce bounceClass;
    public mouseTargetSwivel mouseClass;
    public PredictTrajectory trajectoryClass;


    private bool slo;
    private string powerUpType = "";
    private string playerID = "";

  

    public void setPlayerID(int num)
    {
        if (!photonView.IsMine) return;
        playerID = num.ToString();
        Debug.Log("player ID set to " + playerID);
    }

    public void setMouse(GameObject mouse)
    {
        if (!photonView.IsMine) return;
        //pass mouseTarget GameObject to appropriate classes
        mouseTarget = mouse;
     
        mouseClass.SendMessage("SetMouseAim", mouseTarget);
        trajectoryClass.SendMessage("SetMouseAim", mouseTarget);
    }


    public void setTimeObject(GameObject time)
    {
        slow = time.GetComponent<Slo_Motion>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (powerUpType.Contains("SLOMO") && (Input.GetKeyDown(KeyCode.Space)))
        {
            slo = !slo;
            slow.Activate(slo);
        }

        else if (powerUpType.Contains("SHIELD") && (Input.GetKeyDown(KeyCode.Space)))
        {
            // only allow it to be active if it is turned off
            if (!(shield.activeSelf))
            {
                photonView.RPC("spawnShield", RpcTarget.AllBufferedViaServer);
            }
            
        }
       
      
    }

    [PunRPC]
    public void spawnShield()
    {
        shield.SetActive(true);
        shield.SendMessage("resetFernal", -0.2f);
        powerUpType = "";
    }


    //damage player on collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health -= 5;
        }
    }


    // kill player on 0 health
    private void updateHealth()
    {
        if (health <= 0 )
        {
            var Exploded = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(Exploded, 2f);
            Destroy(this.gameObject, 0f);
            
        }
    }

    public void PowerupAttained(string powerType)
    {
        Debug.Log(powerUpType);
        //reset power type
        //powerUpType = "";

        //if (powerType.Contains(playerID))
        powerUpType = powerType;
    }

    public void getname(string message)
    {
        Debug.Log(message);
    }

}


