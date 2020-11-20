using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameObject player;
    Renderer rend;
    public Material[] mats;

    float bright;
    bool change, disabled, disabled2= false;
    Rigidbody rb;

    private void Start()
    {
      
    }


    private void Update()
    {
        StartCoroutine(Dissolve());
    }

    private void FixedUpdate()
    {
        transform.position = player.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>());

        }

        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("bullet hit shield");
        }

        else if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("zap an enemy");
          
        }
    }

    IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(5f);
        FXAnimation();
    }


    private void FXAnimation()
    {
        //changes fernal and offset, to look like shield is charging up and becoming more solid 
        if (bright > -2.00f && change == false)
        {
            bright -= 0.2f * Time.fixedDeltaTime;
            mats[0].SetFloat("Vector1_EBFAC9DB", bright);
            mats[0].SetFloat("Vector1_CD965979", bright + 2.05f);
            //!!!! change 2.05 value if you want alter brightness to change before material changes 
        }

        else
        {
            change = true;
           // StopAllCoroutines();
            rend.material = mats[1];
            slowlyDestroy();          
        }
    }

    public void resetFernal(float val)
    {
        //set brightness values
        mats[0].SetFloat("Vector1_EBFAC9DB", val);
        mats[0].SetFloat("Vector1_CD965979", 2.37f);
        mats[1].SetFloat("Vector1_7AFF87E4", -0.2f);
        mats[1].SetFloat("Vector1_2E8A09FF", 1);
        bright = val;

        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        rend.material = mats[0];

        //RE-enable rigidbody and animation variables
        change = false;
        disabled= false;
        disabled2 = false;

    }

    private void slowlyDestroy()
    {
     
        // change brightness
        var fernal = mats[1].GetFloat("Vector1_7AFF87E4");
        if (fernal < 2.37f && disabled == false)
        {
            fernal += 0.2f * Time.fixedDeltaTime;
            mats[1].SetFloat("Vector1_7AFF87E4", fernal);
        }

        else
        {
            disabled = true;
        }

        //change dissolve
        var dissolve = mats[1].GetFloat("Vector1_2E8A09FF");
        if (dissolve > -0.9f && disabled2 == false)
        {
            dissolve -= 0.2f * Time.fixedDeltaTime;
            mats[1].SetFloat("Vector1_2E8A09FF", dissolve);
        }

        else
        {
            disabled2 = true;
            Debug.Log("Destroy SHIELD");
            gameObject.SetActive(false);
        }
     
    }


}

