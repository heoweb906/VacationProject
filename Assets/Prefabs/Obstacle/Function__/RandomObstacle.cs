using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObstacle : MonoBehaviour
{
    public Transform[] SpawnPositions;
    public GameObject[] RandomObstacles;

    private void Awake()
    {
        RandomCreateObstacle();
    }

    private void RandomCreateObstacle()
    {
        Instantiate(RandomObstacles[Random.Range(0, 5)], SpawnPositions[0].transform.position, Quaternion.identity);
        Instantiate(RandomObstacles[Random.Range(0, 5)], SpawnPositions[1].transform.position, Quaternion.identity);
        Instantiate(RandomObstacles[Random.Range(0, 5)], SpawnPositions[2].transform.position, Quaternion.identity);
    }
}
