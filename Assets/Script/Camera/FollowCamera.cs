using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // �÷��̾ ������ Ÿ�� ����

    public float smoothSpeed = 0.125f; // ī�޶� �̵��� ����� �ε巯�� �ӵ�

    public Vector3 offsetPosition; // ī�޶�� �÷��̾� ������ ������ ��ġ
    public Vector3 offsetRotation; // ī�޶��� ������ ȸ��

    void LateUpdate()
    {
        if (target == null)
        {
            // Ÿ���� ������ ī�޶� ������ ����
            return;
        }

        Vector3 desiredPosition = target.position + offsetPosition;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.rotation = Quaternion.Euler(target.eulerAngles + offsetRotation); // ī�޶��� ȸ�� ����
    }
}
