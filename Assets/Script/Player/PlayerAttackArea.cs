using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackArea : MonoBehaviour
{
    public PlayerController player;


    void OnTriggerEnter(Collider other)
    {
        // �κ��� �Ǵ��ϱ� ���� ������Ʈ üũ
        AttackRobot_UP robotUp = other.GetComponent<AttackRobot_UP>();
        AttackRobot_DOWN robotDown = other.GetComponent<AttackRobot_DOWN>();
        Obstacle_NextWall newxtWall = other.GetComponent<Obstacle_NextWall>();

        // �ش� ������Ʈ�� ������ �κ��̹Ƿ� ó��
        if (robotUp != null)
        {
            robotUp.Die();
            player.GetFeverGauge();
        }
        else if (robotDown != null)
        {
            robotDown.Die();
            player.GetFeverGauge();
        }
        else if(newxtWall != null)
        {
            newxtWall.BrokeWall();
        }
    }
}
