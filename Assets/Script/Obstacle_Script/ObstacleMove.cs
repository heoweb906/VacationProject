using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    [Header("장애물 등장 위치")]
    public string sAppearancePosition;

    [Header("장애물 정보")]
    public float speed;
    public Vector3 movementDirection;
    public float moveDistance;
    private bool isMoving = false;
    private float movedDistance = 0f; // 이 부분을 추가


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
            // 특정 방향으로 이동
            transform.Translate(movementDirection * speed * Time.deltaTime);
            // 이동한 거리 업데이트
            movedDistance += speed * Time.deltaTime;
        }
    }



    // 장애물 움직이기
    public void ObstacleStart()
    {
        isMoving = true;
    }

    // 움직임 정지
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
