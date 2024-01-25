using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening.Core.Easing;

public class UIController_InGame : MonoBehaviour
{
    [Header("���� ������Ʈ")]
    public PlayerInfomation playerInfo;
    public SoundManager soundManager;
    public GameManager gameManager;


    [Header("UI ����")]
    public GameObject panelOption;
    public GameObject panelGameover;

    public Slider volumeBGMSlider;
    public Slider volumeEffectSlider;

    public TMP_Text text_GoldCnt;


    public void Awake()
    {
        playerInfo = FindObjectOfType<PlayerInfomation>();
        soundManager = FindObjectOfType<SoundManager>();
        gameManager = FindObjectOfType<GameManager>();

        volumeBGMSlider.value = soundManager.fVolumeBGM;
        volumeEffectSlider.value = soundManager.fVolumeEffect;
    }


    public void Update()
    {
        ShowGoldCount();
    }



    // #. ��� ȹ�� �� ��Ȳ ǥ��
    public void ShowGoldCount()
    {
        int playerGold = playerInfo.GoldCnt;
        string formattedGold = string.Format("Gold: {0:D4}", playerGold); // �� �ڸ� ���ڷ� ������
        text_GoldCnt.text = formattedGold; // UI Text�� ��� ���� ǥ��
    }


    // #. �ɼ� â �Ѱ� ���� �Լ�
    public void OnOffOptionPanel()
    {
        volumeBGMSlider.value = soundManager.fVolumeBGM;
        volumeEffectSlider.value = soundManager.fVolumeEffect;

        if (panelOption != null)
        {
            panelOption.SetActive(!panelOption.activeSelf); // �г��� ���¸� ����

            // �г��� Ȱ��ȭ�Ǹ� ������ �Ͻ� ����
            if (panelOption.activeSelf)
            {
                Time.timeScale = 0f; // �ð��� ������Ŵ
            }
            else
            {
                Time.timeScale = 1f; // ������ �ð��� �ٽ� ����
            }
        }
        else
        {
            Debug.LogError("Panel�� �������� �ʾҽ��ϴ�!");
        }
    }



    // #. �׾��� �� ���ӿ����г� ����
    public void OnOffGameOverPanel()
    {
        if (panelGameover != null)
        {
            panelGameover.SetActive(!panelGameover.activeSelf); // �г��� ���¸� ����

            // �г��� Ȱ��ȭ�Ǹ� ������ �Ͻ� ����
            if (panelGameover.activeSelf)
            {
                Time.timeScale = 0f; // �ð��� ������Ŵ
            }
            else
            {
                Time.timeScale = 1f; // ������ �ð��� �ٽ� ����
            }
        }
        else
        {
            Debug.LogError("Panel�� �������� �ʾҽ��ϴ�!");
        }
    }




    // #. ���� ���� ��ư
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }


}
