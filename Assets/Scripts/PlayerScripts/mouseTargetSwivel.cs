using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseTargetSwivel : MonoBehaviour
{
    Vector3 target;
    public GameObject crosshair;
    public GameObject swivelTop;

    void Start()
    {
         //Cursor.visible = false;
        crosshair.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
      
        target = Input.mousePosition;
        target = Camera.main.ScreenToWorldPoint(target);
        crosshair.GetComponent<Rigidbody>().MovePosition(target);



        Vector3 difference = target - swivelTop.transform.position;
        float angleY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
        swivelTop.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0.0f, angleY, 0.0f ));

    }
}


