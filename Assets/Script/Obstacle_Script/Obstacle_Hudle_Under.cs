using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Hudle_Under : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if(!player.bIsSlide)
                {
                    player.TakeDamage();
                }

          
            }
        }
    }
}
