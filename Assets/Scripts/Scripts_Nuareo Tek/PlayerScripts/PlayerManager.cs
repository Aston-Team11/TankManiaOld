using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private int order;

    public float health;
    public GameObject explosion;
    private Slo_Motion slow;
    public GameObject shield;
    public Shield shieldClass;


    public GameObject mouseTarget;

    public mouseTargetSwivel mouseClass;
    public PredictTrajectory trajectoryClass;


    private bool slo;
    private string powerUpType = "";
   // private string playerID = "";
    [SerializeField] private GameObject MySpawners;
    [SerializeField] private GameObject MySystem;

    [SerializeField] private bool poision;
    [SerializeField] private int isPoision;
   


    public void Start()
    {
        SetName();
        SetOrder(Convert.ToInt32(this.gameObject.name));
        setMouse();
        MySystem = GameObject.Find("----SYSTEMS----");
        MySpawners = GameObject.Find("Spawners");
        photonView.RPC("AddPlayerToList", RpcTarget.AllBuffered);
        
    }

    [PunRPC]
    public void AddPlayerToList()
    { 
        MySpawners.GetComponent<Spawner>().addPlayer(this.gameObject);
    }

  
    public void SetName()
    {
        photonView.RPC("UpdateName", RpcTarget.AllBuffered, this.gameObject.name);
    }

    [PunRPC]
    public void UpdateName(string name)
    {
        this.gameObject.name = name;
    }

    // public void setPlayerID(int num)
    // {
    //     if (!photonView.IsMine) return;
    //     playerID = num.ToString();
    //     Debug.Log("player ID set to " + playerID);
    // }

    public void setMouse()
    {
        if (!photonView.IsMine) return;
        //pass mouseTarget GameObject to appropriate classes
        var mouse = Instantiate(mouseTarget, transform.position, transform.rotation);
        mouseTarget = mouse;
        mouseTarget.SetActive(true);
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

        updateHealth();
    }

    [PunRPC]
    public void spawnShield()
    {
        shield.SetActive(true);
        shield.SendMessage("resetFernal", -0.2f);
        powerUpType = "";
    }


   //damage player on collision
 //private void OnCollisionEnter(Collision collision)
 // {
 //     if (collision.gameObject.tag == "aoe")
 //     {
 //          Debug.LogError("it works");
 //      }
 // }


    // kill player on 0 health
    private void updateHealth()
    {
        PosionDamage();

        if (health <= 0)
        {
            var Exploded = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(Exploded, 2f);

            MySystem.SendMessage("Respawn", this.gameObject);
            photonView.RPC("Respawn", RpcTarget.AllBufferedViaServer);

        }
       
    }

    [PunRPC]
    public void Respawn()
    {
        this.gameObject.SetActive(false);
    }

    public void StatReset()
    {
        health = 100;
    }



    public void PowerupAttained(string powerType)
    {
        Debug.Log(powerUpType);
        //reset power type
        //powerUpType = "";

        //if (powerType.Contains(playerID))
        powerUpType = powerType;
    }


    public void DamagePlayer(float dmg)
    {
        health -= dmg;
    }

    public void PosionDamage()
    {
        if(poision == true)
        {
            DamagePlayer(0.05f);
        }
    }


    public float GetHealth()
    {
        return health;
    }

    public void SetOrder(int num)
    {
        order = num;
    }

    public int GetOrder()
    {
        return order;
    }


    public void Setpoisoned(bool state)
    {
        poision = state;

        if (poision == true)
        {
            isPoision++;
            StartCoroutine(endPoison());
        }
        else
        {
            StopCoroutine(endPoison());
        }

    }

    public bool GetPoisoned()
    {
        return poision;
    }

    IEnumerator endPoison()
    {
        yield return new WaitForSeconds(9f);
        if (isPoision > 1)
        {
            poision = false;
            isPoision = 0;
        }
    }


}


