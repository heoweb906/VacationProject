using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour
{
    public GameObject stagePrefab;
    public string sStageName;
    public Transform position_Startpoint;
    public Transform position_Checkpoint;
    public Transform position_Endpoint;


    public PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        Invoke("SetStageFalse", 60f);
    }

    private void Update()
    {
        if(!(player.bIsStun) && player.bIsRun)
        {
            MoveForward();
        } 
    }

    private void SetStageFalse()
    {
        gameObject.SetActive(false);
    }


    // #. 스테이지를 뒤로 이동시키는 ㅎ마수
    void MoveForward()
    {
        transform.Translate(Vector3.forward * -20f * Time.deltaTime);
    }
}
