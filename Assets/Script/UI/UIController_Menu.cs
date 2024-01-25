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

    public Slider volumeBGMSlider;
    public Slider volumeEffectSlider;


 
  


    public void Awake()
    {
        playerInfo = FindObjectOfType<PlayerInfomation>();
        soundManager = FindObjectOfType<SoundManager>();

        volumeBGMSlider.value = soundManager.fVolumeBGM;
        volumeEffectSlider.value = soundManager.fVolumeEffect;
    }



    public void Update()
    {
        // 옵션창이 켜져 있을 때 ESC키를 누르면 옵션창이 꺼짐
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelOption.activeSelf)
            {
                panelOption.SetActive(false); 
            }
        }


    }



    // #. 게임 시작 버튼
    public void GameStartButton()
    {
        SceneManager.LoadScene("Play");
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






    // #. 게임 종료 버튼
    public void QuitGame() 
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
