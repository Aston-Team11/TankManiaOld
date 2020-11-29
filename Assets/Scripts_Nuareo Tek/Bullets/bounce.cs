using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bounce : MonoBehaviourPunCallbacks
{
   
    private GameObject playerOwner;

    public Rigidbody rb;
    Vector3 lastVelocity;
    private int count;
    private bool firstFire;
    public GameObject explosion;
    private GameObject shield;

    // Start is called before the first frame update
    void Start()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        Destroy(this.gameObject, 5f);
        firstFire = true;
        
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
            Debug.Log("player: " + playerOwner.ToString() + "shot himself");
            collision.gameObject.SendMessage("DamagePlayer", 10);
            Explode();
        }

        else if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("bullet impact");
            Explode();
        }

        else if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("player: " + playerOwner.ToString() + " killed an enemy");
            Destroy(this.gameObject, 0f);
        }

        else if (collision.gameObject.tag == "Box")
        {
            collision.gameObject.SendMessage("activatePowerUp", playerOwner);
            Debug.Log("player: " + playerOwner.ToString() + "got a powerup");
            Destroy(this.gameObject, 0f);
        }

        else if (collision.gameObject.tag == "Shield")
        {
            Debug.Log("player: " + playerOwner.ToString() + " shot a shield");
            Explode();
        }

        else if (collision.gameObject.tag == "Non_Collidable")
        {
            Debug.Log("player: " + playerOwner.ToString() + " shot a shield");
            Explode();
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

        //enable collision on shield after a collision is registered
        Physics.IgnoreCollision(shield.GetComponent<Collider>(), gameObject.GetComponent<Collider>(), false);
    }


    private void checkDestroy()
    {
        if(count > 7)
        {
            Destroy(this.gameObject, 0f);
        }
    }

    public void Explode()
    {
        var Exploded =  Instantiate(explosion, transform.position, transform.rotation);
        Destroy(Exploded, 2f);
        Destroy(this.gameObject,0f);
    }


    public void SetPlayerID(GameObject player)
    {
        playerOwner = player;
    }


    public void setShield(GameObject newShield)
    {
        shield = newShield;
    }

}


