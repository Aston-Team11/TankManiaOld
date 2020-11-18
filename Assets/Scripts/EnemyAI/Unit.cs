using UnityEngine;
using System.Collections;


using System;

public class Unit : MonoBehaviour {


	public Transform target;
	public float speed;
	Vector3[] path;
	int targetIndex;

	//
	public Rigidbody rb;
	public float stoppingDistance;
	public GameObject bloodSpray;



	void FixedUpdate()
	{
		try
		{
			//if (Vector3.Distance(transform.position, target.position) >= stoppingDistance)
			//{
				PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
			//}
		}

		catch(IndexOutOfRangeException)
        {
			Debug.Log("enemy dead");
		}
	}

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

	}

	private void Explode()
    {
		Instantiate(bloodSpray, transform.position, transform.rotation);
		Destroy(this.gameObject, 0f);
	}





		public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			targetIndex = 0;
			if(this.gameObject != null)
				StopCoroutine("FollowPath");
				StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath() {
		Vector3 currentWaypoint = path[0];
		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex ++;
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

			transform.LookAt(currentWaypoint);
			rb.AddForce(transform.forward * speed);
			

			yield return null;

		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}