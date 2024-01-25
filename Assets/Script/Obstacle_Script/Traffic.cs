using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic : MonoBehaviour
{
    public float moveSpeed; // �̵� �ӵ�
    private bool bIsMove = false; // �̵� ������ ���θ� ��Ÿ���� �÷���

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
    // �ڷ� �̵� �Լ�
    void MoveBackward()
    {
        float backwardDistance = -moveSpeed * Time.deltaTime;
        Vector3 backwardMovement = new Vector3(0f, 0f, backwardDistance);

        // ��ǥ ��ġ ���
        Vector3 targetPosition = transform.position + backwardMovement;

        // DoTween�� ����� �ε巯�� �ڷ� �̵�
        transform.DOMove(targetPosition, 0.5f); // 0.5�� ���� �ε巴�� �̵�
    }

    // �̵� ���� �Լ�
    public void StartMoving()
    {
        bIsMove = true;
    }

    // �̵� ���� �Լ�
    public void StopMoving()
    {
        bIsMove = false;
    }



}
