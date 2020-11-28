using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {

        //public Transform[] targets; // target to aim for
        public List<Transform> targetsList = new List<Transform>();
        private List<float> dist = new List<float>();
        private int num;            // number target to aim for

        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        public GameObject bloodSpray;


        public void getPlayers(Transform player)
        {
            Debug.Log("works");
            targetsList.Add(player);
            Debug.Log(targetsList[0].position);
        }



        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;

           
        }


        //calculate distance between each enemy and player
        private void Update()
        {
            if (targetsList.Count >= 1)

                // for(int i = 0; i < targetsList.Count; i++)
                // {
                //    float newDist = Vector3.Distance(targetsList[i].transform.position, this.transform.position);
                //    
                //         for(int d = 0; d < dist.Count; d++)
                //         {
                //             if (newDist < dist[d])
                //             {
                //             dist.Add(newDist);
                //             }
                //         }
                // }
              
                target = targetsList[0];
                agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }

    //my code
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
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

        private void Explode()
        {
            Quaternion rot = new Quaternion(transform.rotation.x, transform.rotation.y, 90f, transform.rotation.w);

            Instantiate(bloodSpray, transform.position, bloodSpray.transform.rotation);
            Destroy(this.gameObject, 0f);
        }

    }




}
