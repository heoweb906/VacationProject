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
        // �ɼ�â�� ���� ���� �� ESCŰ�� ������ �ɼ�â�� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelOption.activeSelf)
            {
                panelOption.SetActive(false); 
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






    // #. ���� ���� ��ư
    public void QuitGame() 
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
