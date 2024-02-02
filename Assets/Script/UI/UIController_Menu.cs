using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController_Menu : MonoBehaviour
{
    [Header("���� ������Ʈ")]
    public PlayerInfomation playerInfo;
    public SoundManager soundManager;



    [Header("UI ����")]
    public GameObject panelOption;
    public GameObject panelAchieve;

    public Slider volumeBGMSlider;
    public Slider volumeEffectSlider;


    [Header("���� ����")]
    private float moveRecord;  // �̵� �Ÿ�
    private int itemCntRecord;  // ȹ���� ������ ����
    private int goldCntRecord;  //ȹ���� ��� ������
    private int attackCntRecord; // ���� ���� Ƚ��
    private int obstacleCntRecord; // �پ���� ����� ����

    public GameObject[] Startmove;
    public GameObject[] Startitem;
    public GameObject[] Startgold;
    public GameObject[] Startattack;
    public GameObject[] Startobstacle;



    public void Awake()
    {
        playerInfo = FindObjectOfType<PlayerInfomation>();
        soundManager = FindObjectOfType<SoundManager>();

        volumeBGMSlider.value = soundManager.fVolumeBGM;
        volumeEffectSlider.value = soundManager.fVolumeEffect;


        // #. ���� ���� ������Ʈ
        GetPlayerInfoRecord();
    }



    public void Update()
    {
        // � â�� ���� ���� �� ESCŰ�� ������ Ȱ��ȭ �Ǿ� �ִ� â�� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelOption.activeSelf)
            {
                panelOption.SetActive(false); 
            }
            
            if(panelAchieve.activeSelf)
            {
                panelAchieve.SetActive(false);
            }
            
        }
    }



    // #. ���� ���� ��ư
    public void GameStartButton()
    {
        SceneManager.LoadScene("Play");
    }




    // #. �ɼ� â �Ѱ� ���� �Լ�
    public void OnOffOptionPanel()
    {
        volumeBGMSlider.value = soundManager.fVolumeBGM;
        volumeEffectSlider.value = soundManager.fVolumeEffect;

        if (panelOption != null)
        {
            panelOption.SetActive(!panelOption.activeSelf); // ���� ������ �ݴ�� ����
        }
        else
        {
            Debug.LogError("Panel�� �������� �ʾҽ��ϴ�!");
        }
    }


    // #. ���� â �Ѱ� ���� �Լ�
    public void OnOffAchievePanel()
    {
        if (panelAchieve != null)
        {
            panelAchieve.SetActive(!panelAchieve.activeSelf); // ���� ������ �ݴ�� ����
        }
        else
        {
            Debug.LogError("Panel�� �������� �ʾҽ��ϴ�!");
        }
    }


    // #. ���� ���� ��ư
    public void QuitGame() 
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    private void GetPlayerInfoRecord()
    {
        moveRecord = playerInfo.MoveDistanceRecord;
        itemCntRecord = playerInfo.GetItemCntRecord;
        goldCntRecord = playerInfo.GetGoldCntRecord;
        attackCntRecord = playerInfo.SuccessAttackCntRecord;
        obstacleCntRecord = playerInfo.JumpObstacleCntRecord;


        // #. �̵� �Ÿ�
        int index = playerInfo.MoveDistanceRecord_Bool;
        for (int i = 0; i < index; i++)
        {
            Startmove[i].SetActive(true);
        }

        // #. ������ ȹ�� Ƚ��
        index = playerInfo.GetItemCntRecord_Bool;
        for (int i = 0; i < index; i++)
        {
            Startitem[i].SetActive(true);
        }

        // #. ��� ȹ�淮
        index = playerInfo.GetGoldCntRecord_Bool;
        for (int i = 0; i < index; i++)
        {
            Startgold[i].SetActive(true);
        }

        // #. ���� Ƚ��
        index = playerInfo.SuccessAttackCntRecord_Bool;
        for (int i = 0; i < index; i++)
        {
            Startattack[i].SetActive(true);
        }

        // #. �پ���� ��� ����
        index = playerInfo.JumpObstacleCntRecord_Bool;
        for (int i = 0; i < index; i++)
        {
            Startobstacle[i].SetActive(true);
        }

    }
}
