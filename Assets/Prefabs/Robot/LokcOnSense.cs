using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LokcOnSense : MonoBehaviour
{
    public GameObject[] LockOnObject;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("감지했습니다.");

            foreach (GameObject obj in LockOnObject)
            {
                obj.SetActive(true);
            }

        }
    }
}
