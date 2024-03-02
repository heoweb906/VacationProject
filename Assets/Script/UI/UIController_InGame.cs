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
    public PlayerController player;


    [Header("UI ����")]
    public GameObject panelOption;
    public GameObject panelGameover;

    public Slider volumeBGMSlider;
    public Slider volumeEffectSlider;

    public TMP_Text text_MoveScore;

    public TMP_Text text_GoldCnt; // ��� ��Ȳ ī��Ʈ
    public TMP_Text text_AttackCnt; // ���� ��Ȳ ī��Ʈ
    public GameObject image_MagneticItem; // �ڼ� ������ ������
    public GameObject image_GoldItem; // ��� ������ ������
    public GameObject image_TransparentItem; // ����ȭ ������ ������

    public Image[] Alrmmove;
    public Image[] Alrmitem;
    public Image[] Alrmgold;
    public Image[] Alrmattack;
    public Image[] Alrmobstacle;

    public float moveDistance = 3f;
    public float moveTime = 2f;

    [Header("���� ���� ����")]
    public TMP_Text text_movedistance;
    public TMP_Text text_attackCnt;
    public TMP_Text text_getgoldCnt;

    [Header("���̵� �� ����")]
    public GameObject ImageFadeI;
    public Image ImageFadeIn;
    public float fadeDuration = 2f; // ���̵� �ο� �ɸ��� �ð�


    public void Awake()
    {
        // #. ���̵� ��
        FadeInGameStart(); 

        playerInfo = FindObjectOfType<PlayerInfomation>();
        soundManager = FindObjectOfType<SoundManager>();
        gameManager = FindObjectOfType<GameManager>();

        volumeBGMSlider.value = soundManager.fVolumeBGM;
        volumeEffectSlider.value = soundManager.fVolumeEffect;



        // ���߿� ���� ���� ���۵Ǹ� BGM ���
    }

   

    public void Update()
    {
        // #. UI ������Ʈ
        ShowGoldCount();
        ShowAttackCount();
        ShowItemIcon();

        ClearAhceive_RunDistance();

        text_MoveScore.text = (player.iMoveCnt * 100).ToString();
    }



    // #. ��� ȹ�� �� ��Ȳ ǥ��
    private void ShowGoldCount()
    {
        int playerGold = playerInfo.GoldCnt;
        string formattedGold = string.Format("{0:D4}", playerGold); // �� �ڸ� ���ڷ� ������
        text_GoldCnt.text = formattedGold; // UI Text�� ��� ���� ǥ��
    }

    // #. ���� ���� Ƚ�� ��Ȳ ǥ��
    private void ShowAttackCount()
    {
        int playerAttack = player.iNowFeverGauge;
        string formattedGold = string.Format("{0:D1}", playerAttack); // �� �ڸ� ���ڷ� ������
        text_AttackCnt.text = formattedGold; // UI Text�� ��� ���� ǥ��
    }

    // #. ������ ȹ�� ��Ȳ ������Ʈ
    private void ShowItemIcon()
    {
        image_MagneticItem.SetActive(player.bIsMagnet);
        image_GoldItem.SetActive(player.bIsDoubleCoin);
        image_TransparentItem.SetActive(player.bIsTransparent);
    }





    // #. �ɼ� â �Ѱ� ���� �Լ�
    public void OnOffOptionPanel()
    {
        soundManager.ButtonClickEffect.Play();

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


    private void FadeInGameStart()
    {
        StartCoroutine(FadeInRoutine());
    }
    private IEnumerator FadeInRoutine()
    {
        // ���� ���İ��� 0
        float startAlpha = 1f;
        // ��ǥ ���İ��� 1
        float targetAlpha = 0f;
        // ���� �ð�
        float currentTime = 0f;

        // ���ۺ��� ��ǥ���� �ɸ��� �ð�
        while (currentTime < fadeDuration)
        {
            // �ð� �帧�� ���� ���İ� ����
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / fadeDuration);
            // �̹����� ���İ� ����
            ImageFadeIn.color = new Color(ImageFadeIn.color.r, ImageFadeIn.color.g, ImageFadeIn.color.b, alpha);

            // ���� �ð� ������Ʈ
            currentTime += Time.deltaTime;
            // ���� ������ ���
            yield return null;
        }

        player.FadeInPlayer();
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

                string text_1 = string.Format("�̵��� �Ÿ� : {0}", player.iMoveCnt);
                text_movedistance.text = text_1; // UI Text�� ��� ���� ǥ��

                string text_2 = string.Format("���� Ƚ�� : {0}", player.iAttackCnt);
                text_attackCnt.text = text_2; // UI Text�� ��� ���� ǥ��

                string text_3 = string.Format("ȹ�� ��ȭ : {0}", player.iGoldCnt);
                text_getgoldCnt.text = text_3; // UI Text�� ��� ���� ǥ��
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

        // ȭ�� ���� ũ�⿡ ���� ������ ����Ͽ� �̵� �Ÿ� ����
        float adjustedMoveDistance = moveDistance * (Screen.width / 1920f); // ���⼭ 1920�� ���ϴ� ���� ���� ũ���Դϴ�.

        Vector3 leftTarget = startPosition + Vector3.left * adjustedMoveDistance;

        panelAlrm.transform.DOMove(leftTarget, moveTime)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                // �������� �̵��� ������ �������� ���ư����� Tween�� �̿��Ͽ� �̵�
                panelAlrm.transform.DOMove(startPosition, moveTime)
                    .SetEase(Ease.InOutQuad);
            });
    }


    
}
