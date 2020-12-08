using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviourPunCallbacks
    {

        private Transform target;                                                   // target to aim for
        public GameObject bloodSpray;                                               // bloodspray particle effect

        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling                  

        
        public GameObject spawner;
       


        /// <summary>
        /// @author Riyad K Rahman
        /// sets local target to be the player's transform position
        /// </summary>
        /// <param name="Player"></param>
        public void getPlayers(Transform Player)
        {
            this.target = Player;
        }


        //!!!! Create a distance function which will test the distance of each player. 
        //++++ Which ever player has the lowest distance is set to the target
       


        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;

            spawner = GameObject.FindGameObjectWithTag("EnemySpawn");
            //spawn = GetComponent<Spawner>();
    
           
        }


        
        private void Update()
        { 
            //if there are no valid targets exit this method
            if (target == null) return;

            //!!!! put your calculate distance function here, it's return should be assigned to target (of type Transform) variable

            agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
          
            else
                //stop moving (vertor3.zero) and crouch
                character.Move(Vector3.zero, true, false);
                //!!!! add attacking animation here
                Debug.Log("attack");
        }



        /// <summary>
        /// @author Riyad K Rahman
        /// Defines how this gameobject reacts on collision with other objects in the scene
        /// </summary>
        /// <param name="collision"> This is used to detect shich object this zombie has collided with</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                //!!!! add attacking animation here
                collision.gameObject.GetComponentInParent<PlayerManager>().DamagePlayer(1);
                Debug.Log("attack");
            }

            else if (collision.gameObject.tag == "Bullet")
            {
                Debug.Log("enemy dead");
                Explode();
            }

            else if (collision.gameObject.layer == LayerMask.NameToLayer("Shield"))
            {
                Debug.Log("enemy Vapourised");
                Explode();
            }
        }


        /// <summary>
        /// @author Riyad K Rahman
        /// triggers particle effect and destroys this object/bloodspray object both locally and on the server
        /// </summary>
        private void Explode()
        {
            Quaternion rot = new Quaternion(transform.rotation.x, transform.rotation.y, 90f, transform.rotation.w);

            var blood = Instantiate(bloodSpray, transform.position, bloodSpray.transform.rotation);
            Destroy(blood, 2f);
            // PhotonNetwork.Destroy(this.photonView);
            
            Destroy(this.gameObject, 0f);
            // spawn.enemiesKilled++;
            spawner.SendMessage("IncrementEnemies");
            //spawn.IncrementEnemies();
           

        }

    }




}
