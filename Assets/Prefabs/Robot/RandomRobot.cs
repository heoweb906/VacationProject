using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRobot : MonoBehaviour
{
    public Transform[] SpawnPositions;
    public GameObject[] RobotUp_Down;

    private void Awake()
    {
        RandomCreateRobot();
    }

    private void RandomCreateRobot()
    {
        int Robotnum = Random.Range(0, 2);
        int RobotTransfromNum = Robotnum;
        Instantiate(RobotUp_Down[Robotnum], SpawnPositions[RobotTransfromNum].transform.position, Quaternion.identity);

        Robotnum = Random.Range(0, 2);
        RobotTransfromNum = Robotnum + 2;
        Instantiate(RobotUp_Down[Robotnum], SpawnPositions[RobotTransfromNum].transform.position, Quaternion.identity);

        Robotnum = Random.Range(0, 2);
        RobotTransfromNum = Robotnum + 4;
        Instantiate(RobotUp_Down[Robotnum], SpawnPositions[RobotTransfromNum].transform.position, Quaternion.identity);
    }
}
