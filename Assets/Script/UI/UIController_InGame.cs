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
    public PlayerController player;


    [Header("UI 관련")]
    public GameObject panelOption;
    public GameObject panelGameover;

    public Slider volumeBGMSlider;
    public Slider volumeEffectSlider;

    public TMP_Text text_MoveScore;

    public TMP_Text text_GoldCnt; // 골드 현황 카운트
    public TMP_Text text_AttackCnt; // 공격 현황 카운트
    public GameObject image_MagneticItem; // 자석 아이템 아이콘
    public GameObject image_GoldItem; // 골드 아이템 아이콘
    public GameObject image_TransparentItem; // 투명화 아이템 아이콘

    public Image[] Alrmmove;
    public Image[] Alrmitem;
    public Image[] Alrmgold;
    public Image[] Alrmattack;
    public Image[] Alrmobstacle;

    public float moveDistance = 3f;
    public float moveTime = 2f;

    [Header("게임 오버 관련")]
    public TMP_Text text_movedistance;
    public TMP_Text text_attackCnt;
    public TMP_Text text_getgoldCnt;

    [Header("페이드 인 관련")]
    public GameObject ImageFadeI;
    public Image ImageFadeIn;
    public float fadeDuration = 2f; // 페이드 인에 걸리는 시간


    public void Awake()
    {
        // #. 페이드 인
        FadeInGameStart(); 

        playerInfo = FindObjectOfType<PlayerInfomation>();
        soundManager = FindObjectOfType<SoundManager>();
        gameManager = FindObjectOfType<GameManager>();

        volumeBGMSlider.value = soundManager.fVolumeBGM;
        volumeEffectSlider.value = soundManager.fVolumeEffect;



        // 나중에 메인 게임 시작되면 BGM 재생
    }

   

    public void Update()
    {
        // #. UI 업데이트
        ShowGoldCount();
        ShowAttackCount();
        ShowItemIcon();

        ClearAhceive_RunDistance();

        text_MoveScore.text = (player.iMoveCnt * 100).ToString();
    }



    // #. 골드 획득 시 현황 표시
    private void ShowGoldCount()
    {
        int playerGold = playerInfo.GoldCnt;
        string formattedGold = string.Format("{0:D4}", playerGold); // 네 자리 숫자로 포맷팅
        text_GoldCnt.text = formattedGold; // UI Text에 골드 정보 표시
    }

    // #. 공격 성공 횟수 현황 표시
    private void ShowAttackCount()
    {
        int playerAttack = player.iNowFeverGauge;
        string formattedGold = string.Format("{0:D1}", playerAttack); // 네 자리 숫자로 포맷팅
        text_AttackCnt.text = formattedGold; // UI Text에 골드 정보 표시
    }

    // #. 아이템 획득 현황 업데이트
    private void ShowItemIcon()
    {
        image_MagneticItem.SetActive(player.bIsMagnet);
        image_GoldItem.SetActive(player.bIsDoubleCoin);
        image_TransparentItem.SetActive(player.bIsTransparent);
    }





    // #. 옵션 창 켜고 끄는 함수
    public void OnOffOptionPanel()
    {
        soundManager.ButtonClickEffect.Play();

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


    private void FadeInGameStart()
    {
        StartCoroutine(FadeInRoutine());
    }
    private IEnumerator FadeInRoutine()
    {
        // 시작 알파값은 0
        float startAlpha = 1f;
        // 목표 알파값은 1
        float targetAlpha = 0f;
        // 현재 시간
        float currentTime = 0f;

        // 시작부터 목표까지 걸리는 시간
        while (currentTime < fadeDuration)
        {
            // 시간 흐름에 따른 알파값 변경
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / fadeDuration);
            // 이미지의 알파값 설정
            ImageFadeIn.color = new Color(ImageFadeIn.color.r, ImageFadeIn.color.g, ImageFadeIn.color.b, alpha);

            // 현재 시간 업데이트
            currentTime += Time.deltaTime;
            // 다음 프레임 대기
            yield return null;
        }

        player.FadeInPlayer();
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

                string text_1 = string.Format("이동한 거리 : {0}", player.iMoveCnt);
                text_movedistance.text = text_1; // UI Text에 골드 정보 표시

                string text_2 = string.Format("어택 횟수 : {0}", player.iAttackCnt);
                text_attackCnt.text = text_2; // UI Text에 골드 정보 표시

                string text_3 = string.Format("획득 재화 : {0}", player.iGoldCnt);
                text_getgoldCnt.text = text_3; // UI Text에 골드 정보 표시
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
        soundManager.ButtonClickEffect.Play();

        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ReStart()
    {
        soundManager.ButtonClickEffect.Play();

        Time.timeScale = 1f;
        SceneManager.LoadScene("Play");
    }

    public void ClearAhceive_RunDistance()
    {
        
        if (playerInfo.MoveDistanceRecord >= 300 && playerInfo.MoveDistanceRecord_Bool == 0)
        {
            ShowAcheiveAlrm(Alrmmove[0]);
            playerInfo.MoveDistanceRecord_Bool++;
            PlayerPrefs.SetInt("MoveDistanceRecord_Bool", 1);
        }
        else if (playerInfo.MoveDistanceRecord >= 500 && playerInfo.MoveDistanceRecord_Bool == 1)
        {
            ShowAcheiveAlrm(Alrmmove[1]);
            playerInfo.MoveDistanceRecord_Bool++;
            PlayerPrefs.SetInt("MoveDistanceRecord_Bool", 2);
        }
        else if (playerInfo.MoveDistanceRecord >= 1000 && playerInfo.MoveDistanceRecord_Bool == 2)
        {
            ShowAcheiveAlrm(Alrmmove[2]);
            playerInfo.MoveDistanceRecord_Bool++;
            PlayerPrefs.SetInt("MoveDistanceRecord_Bool", 3);
        }
        else if (playerInfo.MoveDistanceRecord >= 2500 && playerInfo.MoveDistanceRecord_Bool == 3)
        {
            ShowAcheiveAlrm(Alrmmove[3]);
            playerInfo.MoveDistanceRecord_Bool++;
            PlayerPrefs.SetInt("MoveDistanceRecord_Bool", 4);
        }
        else if (playerInfo.MoveDistanceRecord >= 4000 && playerInfo.MoveDistanceRecord_Bool == 4)
        {
            ShowAcheiveAlrm(Alrmmove[4]);
            playerInfo.MoveDistanceRecord_Bool++;
            PlayerPrefs.SetInt("MoveDistanceRecord_Bool", 5);
        } 
    }

    public void ClearAhceive_Other()
    {
        //playerInfo.GetGoldCntRecord;
        if (playerInfo.GetItemCntRecord >= 10 && playerInfo.GetItemCntRecord_Bool == 0)
        {
            ShowAcheiveAlrm(Alrmitem[0]);
            playerInfo.GetItemCntRecord_Bool++;
            PlayerPrefs.SetInt("GetItemCntRecord_Bool", 1);
        }
        else if (playerInfo.GetItemCntRecord >= 20 && playerInfo.GetItemCntRecord_Bool == 1)
        {
            ShowAcheiveAlrm(Alrmitem[1]);
            playerInfo.GetItemCntRecord_Bool++;
            PlayerPrefs.SetInt("GetItemCntRecord_Bool", 2);
        }
        else if (playerInfo.GetItemCntRecord >= 30 && playerInfo.GetItemCntRecord_Bool == 2)
        {
            ShowAcheiveAlrm(Alrmitem[2]);
            playerInfo.GetItemCntRecord_Bool++;
            PlayerPrefs.SetInt("GetItemCntRecord_Bool", 3);
        }
        else if (playerInfo.GetItemCntRecord >= 50 && playerInfo.GetItemCntRecord_Bool == 3)
        {
            ShowAcheiveAlrm(Alrmitem[3]);
            playerInfo.GetItemCntRecord_Bool++;
            PlayerPrefs.SetInt("GetItemCntRecord_Bool", 4);
        }
        else if (playerInfo.GetItemCntRecord >= 100 && playerInfo.GetItemCntRecord_Bool == 4)
        {
            ShowAcheiveAlrm(Alrmitem[4]);
            playerInfo.GetItemCntRecord_Bool++;
            PlayerPrefs.SetInt("GetItemCntRecord_Bool", 5);
        }




        //playerInfo.GetGoldCntRecord;
        if (playerInfo.GetGoldCntRecord >= 100 && playerInfo.GetGoldCntRecord_Bool == 0)
        {
            ShowAcheiveAlrm(Alrmgold[0]);
            playerInfo.GetGoldCntRecord_Bool++;
            PlayerPrefs.SetInt("GetGoldCntRecord_Bool", 1);
        }
        else if (playerInfo.GetGoldCntRecord >= 500 && playerInfo.GetGoldCntRecord_Bool == 1)
        {
            ShowAcheiveAlrm(Alrmgold[1]);
            playerInfo.GetGoldCntRecord_Bool++;
            PlayerPrefs.SetInt("GetGoldCntRecord_Bool", 2);
        }
        else if (playerInfo.GetGoldCntRecord >= 1500 && playerInfo.GetGoldCntRecord_Bool == 2)
        {
            ShowAcheiveAlrm(Alrmgold[2]);
            playerInfo.GetGoldCntRecord_Bool++;
            PlayerPrefs.SetInt("GetGoldCntRecord_Bool", 3);
        }
        else if (playerInfo.GetGoldCntRecord >= 5000 && playerInfo.GetGoldCntRecord_Bool == 3)
        {
            ShowAcheiveAlrm(Alrmgold[3]);
            playerInfo.GetGoldCntRecord_Bool++;
            PlayerPrefs.SetInt("GetGoldCntRecord_Bool", 4);
        }
        else if (playerInfo.GetGoldCntRecord >= 10000 && playerInfo.GetGoldCntRecord_Bool == 4)
        {
            ShowAcheiveAlrm(Alrmgold[4]);
            playerInfo.GetGoldCntRecord_Bool++;
            PlayerPrefs.SetInt("GetGoldCntRecord_Bool", 5);
        }



        //playerInfo.SuccessAttackCntRecord;
        if (playerInfo.SuccessAttackCntRecord >= 100 && playerInfo.SuccessAttackCntRecord_Bool == 0)
        {
            ShowAcheiveAlrm(Alrmattack[0]);
            playerInfo.SuccessAttackCntRecord_Bool++;
            PlayerPrefs.SetInt("SuccessAttackCntRecord_Bool", 1);
        }
        else if (playerInfo.SuccessAttackCntRecord >= 20 && playerInfo.SuccessAttackCntRecord_Bool == 1)
        {
            ShowAcheiveAlrm(Alrmattack[1]);
            playerInfo.SuccessAttackCntRecord_Bool++;
            PlayerPrefs.SetInt("SuccessAttackCntRecord_Bool", 2);
        }
        else if (playerInfo.SuccessAttackCntRecord >= 50 && playerInfo.SuccessAttackCntRecord_Bool == 2)
        {
            ShowAcheiveAlrm(Alrmattack[2]);
            playerInfo.SuccessAttackCntRecord_Bool++;
            PlayerPrefs.SetInt("SuccessAttackCntRecord_Bool", 3);
        }
        else if (playerInfo.SuccessAttackCntRecord >= 100 && playerInfo.SuccessAttackCntRecord_Bool == 3)
        {
            ShowAcheiveAlrm(Alrmattack[3]);
            playerInfo.SuccessAttackCntRecord_Bool++;
            PlayerPrefs.SetInt("SuccessAttackCntRecord_Bool", 4);
        }
        else if (playerInfo.SuccessAttackCntRecord >= 200 && playerInfo.SuccessAttackCntRecord_Bool == 4)
        {
            ShowAcheiveAlrm(Alrmattack[4]);
            playerInfo.SuccessAttackCntRecord_Bool++;
            PlayerPrefs.SetInt("SuccessAttackCntRecord_Bool", 5);
        }



        //playerInfo.JumpObstacleCntRecord;
        if (playerInfo.JumpObstacleCntRecord >= 10 && playerInfo.JumpObstacleCntRecord_Bool == 0)
        {
            ShowAcheiveAlrm(Alrmobstacle[0]);
            playerInfo.JumpObstacleCntRecord_Bool++;
            PlayerPrefs.SetInt("JumpObstacleCntRecord_Bool", 1);
        }
        else if (playerInfo.JumpObstacleCntRecord >= 30 && playerInfo.JumpObstacleCntRecord_Bool == 1)
        {
            ShowAcheiveAlrm(Alrmobstacle[1]);
            playerInfo.JumpObstacleCntRecord_Bool++;
            PlayerPrefs.SetInt("JumpObstacleCntRecord_Bool", 2);
        }
        else if (playerInfo.JumpObstacleCntRecord >= 100 && playerInfo.JumpObstacleCntRecord_Bool == 2)
        {
            ShowAcheiveAlrm(Alrmobstacle[2]);
            playerInfo.JumpObstacleCntRecord_Bool++;
            PlayerPrefs.SetInt("JumpObstacleCntRecord_Bool", 3);
        }
        else if (playerInfo.JumpObstacleCntRecord >= 250 && playerInfo.JumpObstacleCntRecord_Bool == 3)
        {
            ShowAcheiveAlrm(Alrmobstacle[3]);
            playerInfo.JumpObstacleCntRecord_Bool++;
            PlayerPrefs.SetInt("JumpObstacleCntRecord_Bool", 4);
        }
        else if (playerInfo.JumpObstacleCntRecord >= 500 && playerInfo.JumpObstacleCntRecord_Bool == 4)
        {
            ShowAcheiveAlrm(Alrmobstacle[4]);
            playerInfo.JumpObstacleCntRecord_Bool++;
            PlayerPrefs.SetInt("JumpObstacleCntRecord_Bool", 5);
        }
    }



    public void ShowAcheiveAlrm(Image panelAlrm)
    {
        soundManager.audio_AcheiveClearEffect.Play();

        Vector3 startPosition = panelAlrm.transform.position;

        // 화면 가로 크기에 대한 비율을 고려하여 이동 거리 조절
        float adjustedMoveDistance = moveDistance * (Screen.width / 1920f); // 여기서 1920은 원하는 기준 가로 크기입니다.

        Vector3 leftTarget = startPosition + Vector3.left * adjustedMoveDistance;

        panelAlrm.transform.DOMove(leftTarget, moveTime)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                // 왼쪽으로 이동이 끝나면 원점으로 돌아가도록 Tween을 이용하여 이동
                panelAlrm.transform.DOMove(startPosition, moveTime)
                    .SetEase(Ease.InOutQuad);
            });
    }


    
}
