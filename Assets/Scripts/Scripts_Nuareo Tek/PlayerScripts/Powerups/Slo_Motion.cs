using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

    /// <summary>
    /// @author Riyad K Rahman
    /// Handles timeSlowing across all clients when it's <see cref="slowall"/> function is triggered by a player
    /// </summary>
    public class Slo_Motion : MonoBehaviourPunCallbacks

    {
        public float slowFactor = 0.05f;
        public float slowLength = 10f;

        /// <summary>
        ///  @author Riyad K Rahman
        /// increments time back to normla for every frame
        /// </summary>
        private void Update()
        {

            // increment time scale by one 
            Time.timeScale += (1f / slowLength) * Time.unscaledDeltaTime;
            Time.fixedDeltaTime += (0.01f / slowLength) * Time.unscaledDeltaTime;

            // clamp time at 1 so game doesn't speed up
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime, 0f, 0.01f);
        }


        /// <summary>
        ///  @author Riyad K Rahman
        /// method players access to change flow of time
        /// </summary>
        /// <param name="state">if state active then slowmotion <see cref="slowall"/> is triggered across all clients</param>
        public void Activate(bool state)
        {
            // set time to normal if slomo is turned off 
            if (state == true)
            {
                photonView.RPC("slowall", RpcTarget.All);
            }
        }

        /// <summary>
        ///  @author Riyad K Rahman
        ///  method sets timeslow values across all clients
        /// </summary>
        [PunRPC]
        public void slowall()
        {
            Time.timeScale = slowFactor;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

        }

    }

