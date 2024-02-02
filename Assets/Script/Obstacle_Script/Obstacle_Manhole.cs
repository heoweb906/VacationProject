using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Manhole : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.bIsGround)
                {
                    player.bIsStun = true;
                    player.StepManHole();
                }
            }
        }
    }
}
