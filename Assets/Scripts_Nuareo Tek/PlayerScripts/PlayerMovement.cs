using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    public float acceleration, torqueForce;
    private float defaultRotate, defaultSpeed;
    public float steering;
    public Vector2 direction;
  
    public int health = 100;
   

    private void Start()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        defaultSpeed = acceleration;
        defaultRotate = torqueForce;

    }

    // Update is called once per frame
    private void Update()
    {

        direction.x = Input.GetAxisRaw("Vertical");
        direction.y = Input.GetAxisRaw("Horizontal") * steering;

    }


    //handles physics
    private void FixedUpdate()
    {

        rb.AddForce(transform.forward * direction.x * acceleration);
        rb.AddTorque(transform.up * torqueForce * direction.y);

        //check if time has been changed, adjust acceleration if time is slowed
        acceleration = (Time.timeScale != 1) ? defaultSpeed * 10 : defaultSpeed;
        torqueForce = (Time.timeScale != 1) ? defaultRotate * 2 : defaultRotate;

    }

}
