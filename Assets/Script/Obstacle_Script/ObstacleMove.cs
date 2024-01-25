using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    [Header("��ֹ� ���� ��ġ")]
    public string sAppearancePosition;

    [Header("��ֹ� ����")]
    public float speed;
    public Vector3 movementDirection;
    public float moveDistance;
    private bool isMoving = false;
    private float movedDistance = 0f; // �� �κ��� �߰�


    private void Awake()
    {
        switch (sAppearancePosition)
        {
            case "Right":
                movementDirection = Vector3.right;
                break;
            case "Left":
                movementDirection = Vector3.left;
                break;
            case "Up":
                movementDirection = Vector3.up;
                break;
            case "Down":
                movementDirection = Vector3.down;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (isMoving && movedDistance < moveDistance)
        {
            // Ư�� �������� �̵�
            transform.Translate(movementDirection * speed * Time.deltaTime);
            // �̵��� �Ÿ� ������Ʈ
            movedDistance += speed * Time.deltaTime;
        }
    }



    // ��ֹ� �����̱�
    public void ObstacleStart()
    {
        isMoving = true;
    }

    // ������ ����
    public void StopMoving()
    {
        isMoving = false;
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
            }
        }
    }
}
