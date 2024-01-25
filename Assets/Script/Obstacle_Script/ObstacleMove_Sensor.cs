using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove_Sensor : MonoBehaviour
{
    public ObstacleMove obstacle;



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            obstacle.ObstacleStart();
        }
    }
}
