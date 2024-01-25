using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderPass_EndPoint : MonoBehaviour
{
    public UnderPass underpass;
    public bool bIsCheck = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!bIsCheck)
            {
                bIsCheck = true;
                underpass.EndPointReached();
            }

        }
    }

}
