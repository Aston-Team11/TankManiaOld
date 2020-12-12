
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{

   // public float wait_time - 9f;


    void Start()

    {
      StartCoroutine(wait_for_logo());  
    }

    IEnumerator wait_for_logo()

    {
      yield return new WaitForSeconds(7);

      SceneManager.LoadScene(1);
    }
}
