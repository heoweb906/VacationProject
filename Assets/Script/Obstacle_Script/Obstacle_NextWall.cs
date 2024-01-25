using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_NextWall : MonoBehaviour
{







    // #. 벽 깨짐
    // 추후에 기능이나 연출 추가 할 수도 있으니까 ㅇㅇ
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
