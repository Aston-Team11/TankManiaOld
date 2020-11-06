using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    private Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(0.0f, 0.0f, Vertical);
        Vector3 move2 = new Vector3(0.0f, Horizontal * rotationSpeed, 0.0f);
        Quaternion deltaRotation = Quaternion.Euler(move2 * Time.deltaTime);
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * speed;
        }

        //rigid.AddForce(move * speed);
        rigid.MoveRotation(rigid.rotation * deltaRotation);
    
    }
}
