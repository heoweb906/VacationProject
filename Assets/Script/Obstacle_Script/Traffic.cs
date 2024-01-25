using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic : MonoBehaviour
{
    public float moveSpeed; // 이동 속도
    private bool bIsMove = false; // 이동 중인지 여부를 나타내는 플래그

    private void Awake()
    {
        StartMoving();
    }

    private void Update()
    {
        if(bIsMove)
        {
            MoveBackward();
        }
    }
    // 뒤로 이동 함수
    void MoveBackward()
    {
        float backwardDistance = -moveSpeed * Time.deltaTime;
        Vector3 backwardMovement = new Vector3(0f, 0f, backwardDistance);

        // 목표 위치 계산
        Vector3 targetPosition = transform.position + backwardMovement;

        // DoTween을 사용한 부드러운 뒤로 이동
        transform.DOMove(targetPosition, 0.5f); // 0.5초 동안 부드럽게 이동
    }

    // 이동 시작 함수
    public void StartMoving()
    {
        bIsMove = true;
    }

    // 이동 종료 함수
    public void StopMoving()
    {
        bIsMove = false;
    }



}
