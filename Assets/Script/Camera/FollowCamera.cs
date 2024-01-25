using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // 플레이어를 추적할 타겟 변수

    public float smoothSpeed = 0.125f; // 카메라 이동에 사용할 부드러운 속도

    public Vector3 offsetPosition; // 카메라와 플레이어 사이의 오프셋 위치
    public Vector3 offsetRotation; // 카메라의 오프셋 회전

    void LateUpdate()
    {
        if (target == null)
        {
            // 타겟이 없으면 카메라를 따라가지 않음
            return;
        }

        Vector3 desiredPosition = target.position + offsetPosition;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.rotation = Quaternion.Euler(target.eulerAngles + offsetRotation); // 카메라의 회전 설정
    }
}
