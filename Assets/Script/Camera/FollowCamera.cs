using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;


public class FollowCamera : MonoBehaviour
{
    public PlayerController player;

    public Transform target; // 플레이어를 추적할 타겟 변수

    public float smoothSpeed = 0.125f; // 카메라 이동에 사용할 부드러운 속도

    public Vector3 offsetPosition; // 카메라와 플레이어 사이의 오프셋 위치
    public Vector3 offsetRotation; // 카메라의 오프셋 회전


    [Header("카메라 흔들기")]
    public float fShakeDuration;
    public float fShakeMagnitude;
    private Vector3 originalPos;

    void LateUpdate()
    {
        if (target == null)
        {
            // 타겟이 없으면 카메라를 따라가지 않음
            return;
        }

        if(!(player.bIsStun))
        {
            Vector3 desiredPosition = target.position + offsetPosition;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.rotation = Quaternion.Euler(target.eulerAngles + offsetRotation); // 카메라의 회전 설정
        }

        
    }


    public void ShakeCamera()
    {
        StartCoroutine(ShakeCameraCoroutine());
    }

    private IEnumerator ShakeCameraCoroutine()
    {
        originalPos = transform.position;

        float elapsed = 0.0f;

        while (elapsed < fShakeDuration)
        {
            float x = Random.Range(-1f, 1f) * fShakeMagnitude;
            float y = Random.Range(-1f, 1f) * fShakeMagnitude;

            transform.position = new Vector3(transform.position.x + x, transform.position.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
    }

}
