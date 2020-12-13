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
    private int bulletPenetration = 2;

    private Material mat;
    private Renderer rend;
    private bool disabled = false;

    /// <summary>
    /// @author Riyad K Rahman
    /// Start is called before the first frame update
    /// destroys the GameObject 5 seconds after its been fired, 
    /// freezes Y position so that bullet does not fly up or sink down
    /// resets material dissovle value to 1 
    /// <see cref="firstFire"/> is used to allow the bullet to pass through the player's shield on its first collision
    /// </summary>
    void Start()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        Destroy(this.gameObject, 20f);
        firstFire = true;
        rend = GetComponent<Renderer>();
        mat = rend.material;
        mat.SetFloat("Vector1_F9FC2739", 1);
    }

    /// <summary>
    /// @author Riyad K Rahman
    /// dissolves bullet if killed 2 enemies
    /// </summary>
    public void Dissolve()
    {
        this.GetComponent<Collider>().enabled = false;
        var dissolve = mat.GetFloat("Vector1_F9FC2739");
     
        if (dissolve > -0.9f && disabled == false)
        {
            dissolve -= 2.5f * Time.fixedDeltaTime;
            mat.SetFloat("Vector1_F9FC2739", dissolve);
        }
        else
        {
            disabled = true;
            PhotonNetwork.Destroy(this.photonView);
            //Destroy(this.gameObject, 0f);
        }

    }



    /// <summary>
    /// @author Riyad K Rahman
    /// Update is called once per frame
    /// sets last frame velcity to the current velocity, this is neccesary for <see cref="OnCollisionEnter(Collision)"/> when colliding on a reflective surface
    /// calls <see cref="Dissolve"/> to destroy the bullet
    /// </summary>
    void FixedUpdate()
    {
        lastVelocity = rb.velocity;
        if (bulletPenetration <= 0)
        {
            Dissolve();
           
        }
    }


    /// <summary>
    /// @author Riyad K Rahman
    /// Defines how this gameobject reacts on collision with other objects in the scene
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerAddOns")
        {
            if (firstFire == true)
            {
                firstFire = false;
                return;
            }

          //++++Damages the player by 10 hitpoints nad triggers explosion effect
            Debug.Log("player: " + playerOwner.ToString() + "shot himself");
            collision.gameObject.GetComponentInParent<PlayerManager>().DamagePlayer(10);
            Explode();
        }

        else if (collision.gameObject.tag == "Bullet")
        {
          //++++Bullets explode if contact with another bullet
            Debug.Log("bullet impact");
            Explode();
        }

      
        else if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("player: " + playerOwner.ToString() + " killed an enemy");
          //++++Bullet penetration allows the bullet to shoot trhough multiple enemies. For each enemy that is kiled the bullet speed is halved

                lastVelocity = rb.velocity / 2;
                bulletPenetration -= 1;
        }

        else if (collision.gameObject.tag == "Box")
        {
            //++++triggers activatePowerUp function to send a random powerup to the playwerOwner
            collision.gameObject.SendMessage("activatePowerUp", playerOwner);
            Debug.Log("player: " + playerOwner.ToString() + "got a powerup");
            Destroy(this.gameObject, 0f);
        }

        else if (collision.gameObject.tag == "Shield")
        {
            //++++bullet explodes on contact with shield
            Debug.Log("player: " + playerOwner.ToString() + " shot a shield");
            Dissolve();
        }

        else if (collision.gameObject.tag == "Non_Collidable")
        {
            //++++bullet explodes on contact with nonColliable objects 
            Debug.Log("player: " + playerOwner.ToString() + " shot a shield");
            Explode();
        }


        //+++if any other object is hit then the bullet is reflected
        else
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.GetContact(0).normal);
            //++++ the number of bounces is incremented 
            count++;

            //++++ change velocity of rb to new direction
            rb.velocity = direction * Mathf.Max(speed, 0f);
        }

        checkDestroy();

        //++++ enable collision on shield after a collision is registered
        Physics.IgnoreCollision(shield.GetComponent<Collider>(), gameObject.GetComponent<Collider>(), false);
    }

    /// <summary>
    /// @author Riyad K Rahman
    /// if more than 7 bounces are detected then the bullet is destroyed
    /// </summary>
    private void checkDestroy()
    {
        if(count > 3)
        {
           // PhotonNetwork.Destroy(this.photonView);
            Destroy(this.gameObject, 0f);
        }
    }

    /// <summary>
    /// @author Riyad K Rahman
    /// This gameObject is exploded and a server message <see cref="PhotonNetwork.Destroy(PhotonView)"/> is sent to destroy the box for all players
    /// </summary>
    public void Explode()
    {
        var Exploded =  Instantiate(explosion, transform.position, transform.rotation);
        Destroy(Exploded, 2f);
       // PhotonNetwork.Destroy(this.photonView);
        Destroy(this.gameObject,0f);
    }

    /// <summary>
    /// @author Riyad K Rahman
    /// assigns bullet's owner to the player who fired it 
    /// </summary>
    /// <param name="player">The player who fired this bullet</param>
    public void SetPlayerID(GameObject player)
    {
        playerOwner = player;
    }

    /// <summary>
    /// @author Riyad K Rahman
    /// assigns shield's owner to the player who is firing this bullet ( so that this bullet can pass through the shield)
    /// </summary>
    /// <param name="newShield">The player's shiled the bullet should pass through</param>
    public void setShield(GameObject newShield)
    {
        shield = newShield;
    }

}


