using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackArea : MonoBehaviour
{
    public PlayerController player;


    void OnTriggerEnter(Collider other)
    {
        // 로봇을 판단하기 위해 컴포넌트 체크
        AttackRobot_UP robotUp = other.GetComponent<AttackRobot_UP>();
        AttackRobot_DOWN robotDown = other.GetComponent<AttackRobot_DOWN>();
        Obstacle_NextWall newxtWall = other.GetComponent<Obstacle_NextWall>();

        // 해당 컴포넌트가 있으면 로봇이므로 처리
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
