using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wait : MonoBehaviour
{

   // public float wait_time - 9f;


    void Start()

    {
      StartCoroutine(Wait_for_intro());  
    }

    IEnumerator Wait_for_intro()

    {
      yield return new WaitForSeconds(12);

      SceneManager.LoadScene(2);
    }
}
