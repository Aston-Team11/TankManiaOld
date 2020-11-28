using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;


[RequireComponent(typeof(LineRenderer))]


public class PredictTrajectory : MonoBehaviourPunCallbacks
{

    private LineRenderer line;
    private Ray ray;
    private RaycastHit hit;
    private int rayBounce;

    public int reflections;
    public float maxLength;

    private GameObject cursor;
   

    public void SetMouseAim(GameObject Mousetarget)
    {
        cursor = Mousetarget;
    }




    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        //shoots ray forward
        ray = new Ray(transform.position, transform.forward);
        rayBounce = 0;
        //intialise line to one direction (position count is the turning points of the line)
        line.positionCount = 1;
        line.SetPosition(0, transform.position);
        float remainingLength = maxLength;

        for(int i = 0; i< reflections; i++)
        {

            // if a ray hits something
                if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
            {
                
                    rayBounce++;
                    line.positionCount += 1;
                    line.SetPosition(line.positionCount - 1, hit.point);
                    remainingLength -= Vector2.Distance(ray.origin, hit.point);

                // creates new ray from the hitpoint to relfected vector3
                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                   
               // if no "reflective surfaces hit then cancel
                if (hit.collider.tag != "reflectSurface")
                break;
            }

            else
            {
                float distance = (rayBounce >= 1) ? remainingLength : Vector3.Distance(transform.position, cursor.transform.position);
               

                // if no surface at all hit, then carry on line 
                line.positionCount += 1;
                line.SetPosition(line.positionCount - 1, ray.origin + ray.direction * distance);
            }
        }
    }

}
