using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRobot_DOWN : MonoBehaviour
{
    [Header("로봇 능력치")]
    public float raycastDistance; // Raycast의 거리
    public float moveSpeed; // 이동 속도

    private bool playerDetected = false;
    private Transform playerTransform;

    private bool isHit = false;

    private void Update()
    {
        if (playerDetected)
        {
            MoveRobot(Vector3.up);
        }
        else
        {
            DetectPlayerAndDestroy();
        }
    }

    private void MoveRobot(Vector3 direction)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void DetectPlayerAndDestroy()
    {
        RaycastHit hit2;
        if (Physics.Raycast(transform.position, Vector3.up, out hit2, raycastDistance))
        {
            Debug.DrawRay(transform.position, Vector3.up * raycastDistance, Color.red);

            if (hit2.collider.CompareTag("Player"))
            {
                // 플레이어를 감지했을 때의 동작
                playerDetected = true;
                playerTransform = hit2.collider.transform;

                Destroy(gameObject, 2f);
            }
        }
    }

    // #. 죽음 함수
    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && !isHit)
            {
                isHit = true;
                player.TakeDamage();
            }
        }
    }
}
