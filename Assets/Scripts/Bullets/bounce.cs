using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounce : MonoBehaviour
{

    public Rigidbody rb;
    Vector3 lastVelocity;
    private int count;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("explode and damage");
            Explode();
        }

        else if(collision.gameObject.tag == "Bullet")
        {
            Debug.Log("explode");
            Explode();
        }

        else if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("eNemy damage");
            Destroy(this.gameObject, 0f);
        }

        else
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.GetContact(0).normal);
            count++;

            // change velocity of rb to new direction
            rb.velocity = direction * Mathf.Max(speed, 0f);
        }

        checkDestroy();
    }


    private void checkDestroy()
    {
        if(count > 7)
        {
            Destroy(this.gameObject, 0f);
        }

    }


    private void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }


}
