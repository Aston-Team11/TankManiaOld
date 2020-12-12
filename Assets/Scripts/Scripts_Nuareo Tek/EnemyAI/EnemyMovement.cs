
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Transform Player;
	public GameObject bloodSpray;
	public GameObject bombSpray;
	public GameObject radiation;


	// Update is called once per frame
	void FixedUpdate()
    {
        Agent.SetDestination(Player.position);
    }


	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Explode();
		}

		else if (collision.gameObject.tag == "Bullet")
		{
			Debug.Log("enemy dead");
			Death();
		}

	}

	private void Death()
	{
		Quaternion rot = new Quaternion(transform.rotation.x, transform.rotation.y, 90f, transform.rotation.w);
		
		Instantiate(bloodSpray, transform.position, bloodSpray.transform.rotation);
		Destroy(this.gameObject, 0f);
	}

	private void Explode()
	{
		Quaternion rot = new Quaternion(transform.rotation.x, transform.rotation.y, 90f, transform.rotation.w);

		Instantiate(bombSpray, transform.position, bombSpray.transform.rotation);
		Instantiate(radiation, transform.position, radiation.transform.rotation);
		Destroy(this.gameObject, 0f);


	}


}
