using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;


public class FollowCamera : MonoBehaviour
{
    public PlayerController player;

    public Transform target; // �÷��̾ ������ Ÿ�� ����

    public float smoothSpeed = 0.125f; // ī�޶� �̵��� ����� �ε巯�� �ӵ�

    public Vector3 offsetPosition; // ī�޶�� �÷��̾� ������ ������ ��ġ
    public Vector3 offsetRotation; // ī�޶��� ������ ȸ��


    [Header("ī�޶� ����")]
    public float fShakeDuration;
    public float fShakeMagnitude;
    private Vector3 originalPos;

    void LateUpdate()
    {
        if (target == null)
        {
            // Ÿ���� ������ ī�޶� ������ ����
            return;
        }

        if(!(player.bIsStun))
        {
            Vector3 desiredPosition = target.position + offsetPosition;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.rotation = Quaternion.Euler(target.eulerAngles + offsetRotation); // ī�޶��� ȸ�� ����
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
