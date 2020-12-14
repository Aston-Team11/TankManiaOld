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

        [SerializeField] private Transform target;                                                   // target to aim for
        public GameObject[] playerList;
        [SerializeField] private int targetCount;


        [SerializeField] private float minimumDist; //edit this field to mkae the zombie change target
        [SerializeField] private float dist;

        private GameObject spawner; //used to access the spawner script 
        public GameObject Blood_Effect;                                               // bloodspray particle effect

        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling                  


        public GameObject bombSpray;  //this is the effect of the zombie exploding
        public GameObject radiation;  //this is the effect of the explosion remnants
        public GameObject bloodSpray;                                               // bloodspray particle effect


        /// <summary>
        /// Only to be used once
        /// </summary>
        public void TargetPlayer1()
        {
            this.target = GameObject.Find("1").transform;
        }



        /// <summary>
        /// @author Riyad K Rahman
        /// sets local target to be the player's transform position
        /// </summary>
        /// <param name="Player"></param>
        public void getPlayers(Transform Player)
        {
            this.target = Player;
        }


        
        /// <summary>
        /// @author Riyad K Rahman
        /// Calculate's which player is closest then
        /// sets local target to be the player's transform position
        /// </summary>
        /// <param name="Player"></param>
        private void DistanceCalculator()
        {
            playerList = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in playerList)
            {
               
                // if player is alive then check if he is cloesest to the zombie
                if (player.activeSelf == true)
                {
                    dist = Vector3.Distance(player.transform.position, transform.position);
                    //if a different player comes close enough the target changes
                    if (dist <= minimumDist && target != player.transform && targetCount <= 4)
                    {
                        //target = player.transform;
                        photonView.RPC("Retarget", RpcTarget.AllViaServer,player.name);
                    }
                 
                }
            }
        }

        [PunRPC]
        public void Retarget(string name)
        {
            target = GameObject.Find(name).transform;
            targetCount++;
        }



        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;

            spawner = GameObject.FindGameObjectWithTag("EnemySpawn");
            //spawn = GetComponent<Spawner>();
            Blood_Effect = GameObject.Find("ZombieSpawn");


        }


        
        private void Update()
        {
            DistanceCalculator();
           
            //if there are no valid targets exit this method
            if (target == null) return;

           

            agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
          
            else
                //stop moving (vertor3.zero) and crouch
                character.Move(Vector3.zero, false, false);
                //!!!! add attacking animation here
               
        }



        /// <summary>
        /// @author Riyad K Rahman
        /// Defines how this gameobject reacts on collision with other objects in the scene
        /// </summary>
        /// <param name="collision"> This is used to detect shich object this zombie has collided with</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerAddOns")
            {
                //!!!! add attacking animation here
                collision.gameObject.GetComponentInParent<PlayerManager>().DamagePlayer(10);
                Explode();
                Debug.Log("attack");
            }

            else if (collision.gameObject.tag == "Bullet")
            {
                Debug.Log("enemy dead");
                Death();
            }

            else if (collision.gameObject.tag == "Shield")
            {
                Debug.Log("enemy Vapourised");
                Death();
            }
        }


        /// <summary>
        /// @author Riyad K Rahman
        /// triggers particle effect and destroys this object/bloodspray object both locally and on the server
        /// </summary>
        private void Death()
        {
            Quaternion rot = new Quaternion(transform.rotation.x, transform.rotation.y, 90f, transform.rotation.w);

            var blood = Instantiate(bloodSpray, transform.position, bloodSpray.transform.rotation);
            Destroy(blood, 2f);
            PhotonNetwork.Destroy(this.photonView);

            Blood_Effect.GetComponent<AudioSource>().Play();

            Destroy(this.gameObject, 0f);
            // spawn.enemiesKilled++;
            spawner.SendMessage("IncrementEnemies");
            //spawn.IncrementEnemies();
           

        }

        /// <summary>
        /// @author Qabais Mohammed
        /// triggers explosion effect when zombies collide with player.
        /// removed zombie object and local visual effects
        /// </summary>
        private void Explode()
        {
            Quaternion rot = new Quaternion(transform.rotation.x, transform.rotation.y, 90f, transform.rotation.w);

            //call explosion effect
            var zExplosion = Instantiate(bombSpray, transform.position, bombSpray.transform.rotation);
            Destroy(zExplosion, 2f);
            //call toxic effect 
            var zRadiation = Instantiate(radiation, transform.position, radiation.transform.rotation);
            //kill zombie after contact
            Destroy(this.gameObject, 0f);
            PhotonNetwork.Destroy(this.photonView);

            // spawn.enemiesKilled++;
            spawner.SendMessage("IncrementEnemies");

        }
    }




}
