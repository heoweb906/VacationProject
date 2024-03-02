using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRobot_UP : MonoBehaviour
{
    private PlayerController pplayer;

    [Header("�κ� �ɷ�ġ")]
    public float raycastDistance; // Raycast�� �Ÿ�
    public float moveSpeed; // �̵� �ӵ�

    public bool playerDetected = false;
    private Transform playerTransform;

    private bool isHit = false;


    public float frontDistance;

    private void Awake()
    {
        pplayer = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (playerDetected)
        {
            MoveRobot(Vector3.down);
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
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position + transform.forward * frontDistance;
        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance))
        {
            Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.red);

            if (hit.collider.CompareTag("Player"))
            {
                // �÷��̾ �������� ���� ����
                playerDetected = true;
                playerTransform = hit.collider.transform;

                Destroy(gameObject, 2f);
            }
        }
    }

   
    // #. ���� �Լ�
    public void Die()
    {
        pplayer.RecordAttackCnt();
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
