using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform Player;
    public Vector3 offset;

    private void Update()
    {
        transform.position = Player.position + offset;
        Quaternion rot = new Quaternion(transform.rotation.x, Player.rotation.y, Player.rotation.z, -1f);
    }

    public void getPlayer(GameObject player0)
    {
        Player = player0.transform;
        Debug.Log("set to: " + player0.ToString());
    }


}