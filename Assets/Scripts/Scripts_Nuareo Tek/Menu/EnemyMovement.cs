
//using UnityEngine;
//using UnityEngine.AI;
//
//public class EnemyMovement : MonoBehaviour
//{
//    public NavMeshAgent Agent;
//    public Transform Player;
//	public GameObject bloodSpray;
//
//	// Update is called once per frame
//	void FixedUpdate()
//    {
//        Agent.SetDestination(Player.position);
//    }
//
//
//	private void OnCollisionEnter(Collision collision)
//	{
//		if (collision.gameObject.tag == "Player")
//		{
//			Debug.Log("attack");
//		}
//
//		else if (collision.gameObject.tag == "Bullet")
//		{
//			Debug.Log("enemy dead");
//			Explode();
//		}
//
//	}
//
//	private void Explode()
//	{
//		Quaternion rot = new Quaternion(transform.rotation.x, transform.rotation.y, 90f, transform.rotation.w);
//		
//		Instantiate(bloodSpray, transform.position, bloodSpray.transform.rotation);
//		Destroy(this.gameObject, 0f);
//	}
//
//}
