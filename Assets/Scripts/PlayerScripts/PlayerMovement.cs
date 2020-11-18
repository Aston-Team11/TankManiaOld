using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    public float acceleration, MaxSpeed, torqueForce;
    public float steering, driftFactor;
    public Vector2 direction;
    Vector3 movement;

    public int health = 100;
    public GameObject explosion;

    private void Start()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    // Update is called once per frame
    void Update()
    {

        direction.x = Input.GetAxisRaw("Vertical");
        direction.y = Input.GetAxisRaw("Horizontal") * steering;

        //ShootPlayer();
    }


    //handles physics
    private void FixedUpdate()
    {

        rb.AddForce(transform.forward * direction.x * acceleration);
        rb.AddTorque(transform.up * torqueForce * direction.y);
        updateHealth();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health -= 5;
        }
    }

    private void updateHealth()
    {
        if (health.Equals(0))
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(this.gameObject,0f);
        }
    }

}
