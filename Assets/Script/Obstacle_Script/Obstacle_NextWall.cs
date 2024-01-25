using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_NextWall : MonoBehaviour
{







    // #. �� ����
    // ���Ŀ� ����̳� ���� �߰� �� ���� �����ϱ� ����
    public void BrokeWall()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.DiePlayer();
            }
        }
    }
}
