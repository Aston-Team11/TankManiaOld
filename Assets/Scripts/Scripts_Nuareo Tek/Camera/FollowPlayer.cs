using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Riyad K Rahman
/// Handles camera movement to follow player, Uses an offset to properly position camera
/// </summary>
public class FollowPlayer : MonoBehaviour
{
    private Transform Player;
    public Vector3 offset;

    private void Update()
    {
        transform.position = Player.position + offset;
        Quaternion rot = new Quaternion(transform.rotation.x, Player.rotation.y, Player.rotation.z, -1f);
    }

    /// <summary>
    /// @author Riyad K Rahman
    /// sets transform to follow as player's transform position
    /// </summary>
    /// <param name="player0"></param>
    public void getPlayer(GameObject player0)
    {
        Player = player0.transform;
        Debug.Log("set to: " + player0.ToString());
    }

    //!!!! Create a function to rotate camera using player inputs or dynamically rotate it using raycasts to detect collisions against buildings 
}