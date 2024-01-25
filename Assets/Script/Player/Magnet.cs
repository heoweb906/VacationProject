using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Magnet : MonoBehaviour
{
    public Transform position_Player;

    public float pullSpeed = 1.0f; // 당기는 속도
    public float activationRange = 5.0f; // 마그넷 활성 범위
    public LayerMask coinLayer; // Coin 레이어마스크

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, activationRange, coinLayer);

        foreach (Collider collider in colliders)
        {
            Vector3 direction = (position_Player.position - collider.transform.position).normalized;
            float distance = Vector3.Distance(position_Player.position, collider.transform.position);
            float pullForce = Mathf.Clamp(1.0f / distance, 0.0f, 1.0f) * pullSpeed;

            // 서서히 플레이어 쪽으로 당김
            collider.attachedRigidbody.MovePosition(collider.transform.position + direction * pullForce * Time.deltaTime);
        }
    }
}
