using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // 위의 과정을 반복하는 알고리즘 제작
    // 몇 개의 스테이지를 통과했는지 카운트 해서 Time을 가속해야 함, 임의로 1.1배씩 가속
    // 몇 개의 스테이지를 통과했는지 카운트 해서 Time을 가속해야 함, 임의로 1.1배씩 가속
    // 몇 개의 스테이지를 통과했는지 카운트 해서 Time을 가속해야 함, 임의로 1.1배씩 가속
    // 몇 개의 스테이지를 통과했는지 카운트 해서 Time을 가속해야 함, 임의로 1.1배씩 가속
    // @@@@@@ 정확한 가속 치수는 기획자에게 전달할 것 


    public Transform gamePosition; // 게임 최초 실행시 위치

    [SerializeField] private Transform checkPosition_1;
    [SerializeField] private Transform checkPosition_2;
    [SerializeField] private Transform endPosition;
    public StageInfo[] stageInfos;

    public GameObject player;

    public void Awake()
    {
        CreateGameStage();
    }

    public void Update()
    {
        if (player.transform.position.z > checkPosition_1.position.z)
        {
            CreateNewStage();
        }
    }


    // #. 게임 최초 실행 시 맵 생성
    private void CreateGameStage()
    {
        int randomIndex = Random.Range(0, stageInfos.Length); // 스테이지 랜덤으로 선택

        // 임시로 0번 스테이지를 최초 생성, 추후에 최초의 스테이지도 랜덤으로 생성하도록 수정 
        StageInfo newStage = Instantiate(stageInfos[randomIndex], gamePosition.position, Quaternion.identity);
        checkPosition_1 = newStage.position_Checkpoint;
        SetManagerInfo(newStage);

        randomIndex = Random.Range(0, stageInfos.Length); // 스테이지 랜덤으로 선택
        newStage = Instantiate(stageInfos[randomIndex], endPosition.position, Quaternion.identity);
        SetManagerInfo(newStage);
    }

    // #. 다음 맵 생성 함수
    private void CreateNewStage()
    {
        checkPosition_1 = checkPosition_2;

        int randomIndex = Random.Range(0, stageInfos.Length);  // 스테이지 랜덤으로 선택
        StageInfo newStage = Instantiate(stageInfos[randomIndex], endPosition.position, Quaternion.identity);
        SetManagerInfo(newStage);
    }


    // 매니저에 현재 스테이지의 정보 저장
    private void SetManagerInfo(StageInfo stageInfos)
    {
        if (player.transform.position.z < checkPosition_1.position.z)
        {
            checkPosition_2 = stageInfos.position_Checkpoint;
        }
        else
        {
            checkPosition_1 = stageInfos.position_Checkpoint;
        }

        endPosition = stageInfos.position_Endpoint;
    }


}
