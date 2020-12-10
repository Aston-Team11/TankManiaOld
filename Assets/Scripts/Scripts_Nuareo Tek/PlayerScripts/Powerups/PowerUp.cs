using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


    /// <summary>
    /// @author Riyad K Rahman
    /// class handles randomising and sending powerups to a player
    /// </summary>
    public class PowerUp : MonoBehaviourPunCallbacks
    {
        public GameObject explosion;            // the paricle effect for explosion
    public GameObject explosion_soundEffect;  // the explosion sound effect for the power up boxes

    public void Start(){
   // explosionSound = GetComponent<AudioSource>();
   explosion_soundEffect = GameObject.FindGameObjectWithTag("PowerUpExplosion"); //the tag is connected to the Explosives object sound in the map object.
    }
        /// <summary>
        /// @author Riyad K Rahman
        /// when this gameObject collides with a bullet the <see cref="Explode"/> function is called to break the box
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                Debug.Log("givePowerUp");
                Explode();
            
        }

        }

        /// <summary>
        /// @author Riyad K Rahman
        /// This gameObject is exploded and a server message (<see cref="sendDestroy"/> is sent to destroy the box for all players
        /// </summary>
        private void Explode()
        {
            var Exploded = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(Exploded, 2f);
       // explosionSound.Play();
        explosion_soundEffect.GetComponent<AudioSource>().Play();        //play the explosion sound effect.
        photonView.RPC("sendDestroy", RpcTarget.AllBufferedViaServer);
        //playSound();
    }

        /// <summary>
        /// @author Riyad K Rahman
        /// PUNRPC <see cref="PunRPC"/> is used to identify methods as server methods. Photon uses this function to destroy this gameObject
        /// </summary>
        [PunRPC]
        public void sendDestroy()
        {
        Destroy(this.gameObject);
        }

        /// <summary>
        /// @author Riyad K Rahman
        /// 'result' picks a random number 1 or 2. (at the moment this is only 2 powerups, but will be many more in a later date)
        /// A case statement then sends a message to the player <see cref="PlayerManager.PowerupAttained(string)"/> with a string of the type of powerup being sent 
        /// </summary>
        /// <param name="player"> is used to identify which player to send the powerup to</param>
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


        }
    IEnumerator playSound()
    {
        yield return new WaitForSeconds(8f);
        photonView.RPC("sendDestroy", RpcTarget.AllBufferedViaServer);
    }

}
