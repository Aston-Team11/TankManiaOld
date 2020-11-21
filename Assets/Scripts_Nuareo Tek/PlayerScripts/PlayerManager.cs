using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public int health = 100;
    public GameObject explosion;
    public Slo_Motion slow;
    public GameObject shield;
    public Shield shieldClass;

    private bool slo;
    private string powerUpType = "";
    private string playerID = "1";

  

    private void Update()
    {
     
        if (powerUpType.Contains("SLOMO") && (Input.GetKeyDown(KeyCode.Space)))
        {
            slo = !slo;
        }

        else if (powerUpType.Contains("SHIELD") && (Input.GetKeyDown(KeyCode.Space)))
        {
            // only allow it to be active if it is turned off
            if (!(shield.activeSelf))
            {
                shield.SetActive(true);
                shield.SendMessage("resetFernal", -0.2f);
                powerUpType = "";
            }
            
        }

        slow.Activate(slo);
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
        if (health.Equals(0))
        {
            var Exploded = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(Exploded, 2f);
            Destroy(this.gameObject, 0f);
            
        }
    }

    public void PowerupAttained(string powerType)
    {
        //reset power type
        powerUpType = "";

        if (powerType.Contains(playerID))
        powerUpType = powerType;
    }
}
