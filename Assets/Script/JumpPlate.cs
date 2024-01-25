using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlate : MonoBehaviour
{
    public float fJumpForce;

    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
       

        if (player != null)
        {
            player.Jump(fJumpForce);
        }

    }
}
