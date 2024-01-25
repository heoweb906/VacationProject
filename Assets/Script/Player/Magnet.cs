using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Magnet : MonoBehaviour
{
    public Transform position_Player;

    public float pullSpeed = 1.0f; // ���� �ӵ�
    public float activationRange = 5.0f; // ���׳� Ȱ�� ����
    public LayerMask coinLayer; // Coin ���̾��ũ

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

            // ������ �÷��̾� ������ ���
            collider.attachedRigidbody.MovePosition(collider.transform.position + direction * pullForce * Time.deltaTime);
        }
    }
}
