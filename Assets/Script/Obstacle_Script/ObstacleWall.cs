using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleWall : MonoBehaviour
{
    [Header("�� ��ġ - �� / ��")]
    public float fPushDirection;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage();

                player.TouchWall(fPushDirection);
            }
        }
    }

}
