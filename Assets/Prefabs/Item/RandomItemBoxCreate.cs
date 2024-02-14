using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemBoxCreate : MonoBehaviour
{
    public GameObject[] randomItemBox;

    void Start()
    {
        CreateItemBox();
    }

    private void CreateItemBox()
    {
        Instantiate(randomItemBox[Random.Range(0, 5)], transform.position, Quaternion.Euler(-90f, 0f, 0f));
    }


}
