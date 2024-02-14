using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController_Menu : MonoBehaviour
{
    [Header("참고 오브젝트")]
    public PlayerInfomation playerInfo;
    public SoundManager soundManager;



    [Header("UI 관련")]
    public GameObject panelOption;
    public GameObject panelAchieve;

    public Slider volumeBGMSlider;
    public Slider volumeEffectSlider;

    public GameObject ImageFadeOu;
    public Image ImageFadeOut;
    public float fadeDuration = 2f; // 페이드 아웃에 걸리는 시간


    [Header("업적 관련")]
    private float moveRecord;  // 이동 거리
    private int itemCntRecord;  // 획득한 아이템 개수
    private int goldCntRecord;  //획득한 골드 갸개수
    private int attackCntRecord; // 공격 성공 횟수
    private int obstacleCntRecord; // 뛰어넘은 허들의 개수

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


        // #. 업적 정보 업데이트
        GetPlayerInfoRecord();
    }




    public void Update()
    {
        // 어떤 창이 켜져 있을 때 ESC키를 누르면 활성화 되어 있는 창이 꺼짐
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


    // #. 게임 시작 버튼
    public void GameStartButton()
    {
        ImageFadeOu.SetActive(true);
        StartCoroutine(FadeOutRoutine());
    }
    private IEnumerator FadeOutRoutine()
    {
        // 시작 알파값은 0
        float startAlpha = 0f;
        // 목표 알파값은 1
        float targetAlpha = 1f;
        // 현재 시간
        float currentTime = 0f;

        // 시작부터 목표까지 걸리는 시간
        while (currentTime < fadeDuration)
        {
            // 시간 흐름에 따른 알파값 변경
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / fadeDuration);
            // 이미지의 알파값 설정
            ImageFadeOut.color = new Color(ImageFadeOut.color.r, ImageFadeOut.color.g, ImageFadeOut.color.b, alpha);

            // 현재 시간 업데이트
            currentTime += Time.deltaTime;
            // 다음 프레임 대기
            yield return null;
        }

        // 페이드 아웃이 완료되면 씬 로드
        SceneManager.LoadScene("Loading");
    }












    // #. 옵션 창 켜고 끄는 함수
    public void OnOffOptionPanel()
    {
        volumeBGMSlider.value = soundManager.fVolumeBGM;
        volumeEffectSlider.value = soundManager.fVolumeEffect;

        if (panelOption != null)
        {
            panelOption.SetActive(!panelOption.activeSelf); // 현재 상태의 반대로 설정
        }
        else
        {
            Debug.LogError("Panel이 지정되지 않았습니다!");
        }
    }


    // #. 업적 창 켜고 끄는 함수
    public void OnOffAchievePanel()
    {
        if (panelAchieve != null)
        {
            panelAchieve.SetActive(!panelAchieve.activeSelf); // 현재 상태의 반대로 설정
        }
        else
        {
            Debug.LogError("Panel이 지정되지 않았습니다!");
        }
    }


    // #. 게임 종료 버튼
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


        // #. 이동 거리
        int index = playerInfo.MoveDistanceRecord_Bool;
        for (int i = 0; i < index; i++)
        {
            Startmove[i].SetActive(true);
        }

        // #. 아이템 획득 횟수
        index = playerInfo.GetItemCntRecord_Bool;
        for (int i = 0; i < index; i++)
        {
            Startitem[i].SetActive(true);
        }

        // #. 골드 획득량
        index = playerInfo.GetGoldCntRecord_Bool;
        for (int i = 0; i < index; i++)
        {
            Startgold[i].SetActive(true);
        }

        // #. 어택 횟수
        index = playerInfo.SuccessAttackCntRecord_Bool;
        for (int i = 0; i < index; i++)
        {
            Startattack[i].SetActive(true);
        }

        // #. 뛰어넘은 허들 개수
        index = playerInfo.JumpObstacleCntRecord_Bool;
        for (int i = 0; i < index; i++)
        {
            Startobstacle[i].SetActive(true);
        }

    }
}
