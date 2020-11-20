using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseTargetSwivel : MonoBehaviour
{
    Vector3 target;
    public GameObject crosshair;
    public GameObject swivelTop;

    private RaycastHit hit;

    void Start()
    {
         //Cursor.visible = false;
        crosshair.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // shoots ray from camera to mouse position;
        target = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(target);


        // checks if raycase hit something, then assign the postion to target vector
        if (Physics.Raycast(ray, out hit))
        {
            target = hit.point;
        }
      
     

        // visualise mouse cursor with target position
        crosshair.GetComponent<Rigidbody>().MovePosition(target);

        // sviwel the tank's top to the mouse target position
        Vector3 difference = target - swivelTop.transform.position;
        float angleY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;

        // rotate slowly towards mouse Y postition
        Quaternion rotTarget = Quaternion.Euler(0.0f, angleY, 0.0f);
        swivelTop.transform.rotation = Quaternion.RotateTowards(swivelTop.transform.rotation, rotTarget, 20f);
    }
}


