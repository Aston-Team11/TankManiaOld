using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swivel : MonoBehaviour
{

    public float rotationSpeed;
    public Rigidbody rb;

    private void Start()
    {
     
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.J))
        {
            
            rb.AddTorque(transform.up * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.K))
        {
            rb.AddTorque(transform.up * -rotationSpeed);
        }
    }
}
