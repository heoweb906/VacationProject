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
    [Header("참고 오브젝트")]
    public PlayerInfomation playerInfo;
    public SoundManager soundManager;
    public GameManager gameManager;


    [Header("UI 관련")]
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



    // #. 골드 획득 시 현황 표시
    public void ShowGoldCount()
    {
        int playerGold = playerInfo.GoldCnt;
        string formattedGold = string.Format("Gold: {0:D4}", playerGold); // 네 자리 숫자로 포맷팅
        text_GoldCnt.text = formattedGold; // UI Text에 골드 정보 표시
    }


    // #. 옵션 창 켜고 끄는 함수
    public void OnOffOptionPanel()
    {
        volumeBGMSlider.value = soundManager.fVolumeBGM;
        volumeEffectSlider.value = soundManager.fVolumeEffect;

        if (panelOption != null)
        {
            panelOption.SetActive(!panelOption.activeSelf); // 패널의 상태를 반전

            // 패널이 활성화되면 게임을 일시 정지
            if (panelOption.activeSelf)
            {
                Time.timeScale = 0f; // 시간을 정지시킴
            }
            else
            {
                Time.timeScale = 1f; // 정지된 시간을 다시 시작
            }
        }
        else
        {
            Debug.LogError("Panel이 지정되지 않았습니다!");
        }
    }



    // #. 죽었을 때 게임오버패널 띄우기
    public void OnOffGameOverPanel()
    {
        if (panelGameover != null)
        {
            panelGameover.SetActive(!panelGameover.activeSelf); // 패널의 상태를 반전

            // 패널이 활성화되면 게임을 일시 정지
            if (panelGameover.activeSelf)
            {
                Time.timeScale = 0f; // 시간을 정지시킴
            }
            else
            {
                Time.timeScale = 1f; // 정지된 시간을 다시 시작
            }
        }
        else
        {
            Debug.LogError("Panel이 지정되지 않았습니다!");
        }
    }




    // #. 게임 시작 버튼
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }


}
