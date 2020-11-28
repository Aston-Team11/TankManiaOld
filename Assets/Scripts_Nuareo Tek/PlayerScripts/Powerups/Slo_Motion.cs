using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Slo_Motion : MonoBehaviourPunCallbacks

{
    public float slowFactor = 0.05f;
    public float slowLength = 10f;

    private void Update()
    {
        
      // increment time scale by one 
        Time.timeScale += (1f / slowLength) * Time.unscaledDeltaTime;
        Time.fixedDeltaTime += (0.01f / slowLength) * Time.unscaledDeltaTime;

      // clamp time at 1 so game doesn't speed up
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime, 0f, 0.01f);
    }

    public void Activate(bool state)
    {
        // set time to normal if slomo is turned off 
        if (state == true){
            photonView.RPC("slowall", RpcTarget.All);
        }
        else { 
           // photonView.RPC("unslowAll", RpcTarget.All);
        }

      
    }
 
    [PunRPC]
    public void slowall()
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
       
    }

   


  //  [PunRPC]
   // public void unslowAll()
  //  {
   //     Time.timeScale = 1;
   // }
}
