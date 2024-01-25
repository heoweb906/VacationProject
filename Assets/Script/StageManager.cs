using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // ���� ������ �ݺ��ϴ� �˰��� ����
    // �� ���� ���������� ����ߴ��� ī��Ʈ �ؼ� Time�� �����ؾ� ��, ���Ƿ� 1.1�辿 ����
    // �� ���� ���������� ����ߴ��� ī��Ʈ �ؼ� Time�� �����ؾ� ��, ���Ƿ� 1.1�辿 ����
    // �� ���� ���������� ����ߴ��� ī��Ʈ �ؼ� Time�� �����ؾ� ��, ���Ƿ� 1.1�辿 ����
    // �� ���� ���������� ����ߴ��� ī��Ʈ �ؼ� Time�� �����ؾ� ��, ���Ƿ� 1.1�辿 ����
    // @@@@@@ ��Ȯ�� ���� ġ���� ��ȹ�ڿ��� ������ �� 


    public Transform gamePosition; // ���� ���� ����� ��ġ

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


    // #. ���� ���� ���� �� �� ����
    private void CreateGameStage()
    {
        int randomIndex = Random.Range(0, stageInfos.Length); // �������� �������� ����

        // �ӽ÷� 0�� ���������� ���� ����, ���Ŀ� ������ ���������� �������� �����ϵ��� ���� 
        StageInfo newStage = Instantiate(stageInfos[randomIndex], gamePosition.position, Quaternion.identity);
        checkPosition_1 = newStage.position_Checkpoint;
        SetManagerInfo(newStage);

        randomIndex = Random.Range(0, stageInfos.Length); // �������� �������� ����
        newStage = Instantiate(stageInfos[randomIndex], endPosition.position, Quaternion.identity);
        SetManagerInfo(newStage);
    }

    // #. ���� �� ���� �Լ�
    private void CreateNewStage()
    {
        checkPosition_1 = checkPosition_2;

        int randomIndex = Random.Range(0, stageInfos.Length);  // �������� �������� ����
        StageInfo newStage = Instantiate(stageInfos[randomIndex], endPosition.position, Quaternion.identity);
        SetManagerInfo(newStage);
    }


    // �Ŵ����� ���� ���������� ���� ����
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
