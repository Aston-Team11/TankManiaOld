using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounce : MonoBehaviour
{
    private string playerID = "";
    public Rigidbody rb;
    Vector3 lastVelocity;
    private int count;
    private bool firstFire;
    public GameObject explosion;
    

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
        //pass through shield on first shot
        if (collision.gameObject.layer == 8 && firstFire == true)
        {
            firstFire = false;
            //Physics.IgnoreLayerCollision(0, 8);
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>(), false);
            return;
        } 

       

        if (collision.gameObject.tag == "Player")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("player: " + playerID + "shot himself");
            Explode();
        }

        else if(collision.gameObject.tag == "Bullet")
        {
            Debug.Log("bullet impact");
            Explode();
        }

        else if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("player: " + playerID + " killed an enemy");
            Destroy(this.gameObject, 0f);
        }

        else if (collision.gameObject.tag == "Box")
        {
            Debug.Log("player: " + playerID + "got a powerup");
            collision.gameObject.SendMessage("activatePowerUp",playerID);
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
        var Exploded =  Instantiate(explosion, transform.position, transform.rotation);
        Destroy(Exploded, 2f);
        Destroy(this.gameObject);
      
    }


    public void SetPlayerID(string id)
    {
        playerID = id;
    }

}


