using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooting : MonoBehaviourPunCallbacks
{
    public GameObject theBullet;
    public GameObject top;

    public int bulletSpeed;
    // the player who shot the bullet is the parent
    public GameObject parent;

    public bool shootAble = true;
    public float waitBeforeNextShot = 0.25f;
    public GameObject Shield;

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetButtonDown("Fire1"))
        {
            if (shootAble)
            {
                shootAble = false;
                // send shoot function for every player
                photonView.RPC("Shoot", RpcTarget.All);
                //Shoot();
                StartCoroutine(ShootingYield());
            }
        }

    }

    IEnumerator ShootingYield()
    {
        yield return new WaitForSeconds(waitBeforeNextShot);
        shootAble = true;
    }

    // allow shoot to be called on other machines
    [PunRPC]
    public void Shoot()
    {
            Quaternion rot = top.transform.rotation;
            float angle = 90 * Mathf.Deg2Rad;

            rot.Set(rot.x, rot.y, angle, 1);
            var bullet = Instantiate(theBullet, transform.position, rot);
            bullet.SendMessage("SetPlayerID", parent);
             bullet.SendMessage("setShield", Shield);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
       
        if (Shield.activeSelf)
        {
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), Shield.GetComponent<Collider>(), true);
        }


            rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
       

        //!!!! space for fx
    }

   

}
