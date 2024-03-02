using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{
    [Header("���� ����")]
    public float fVolumeBGM;
    public float fVolumeEffect;


    [Header("���� - BGM")]
    public AudioSource audio_MainMenuBGM;
    public AudioSource audio_GameOverBGM;

    [Header("���� - Effect Sound")]
    public AudioSource audio_AcheiveClearEffect;
    public AudioSource audio_AttackEffect;
    public AudioSource ButtonClickEffect;
    public AudioSource CharacterHitEffect;
    public AudioSource CharacterMoveEffect;
    public AudioSource GetCoinEffect;



    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Volume_BGMSound"))
        {
            fVolumeBGM = 0.5f;
            PlayerPrefs.SetFloat("Volume_BGMSound",0.5f);
        }
        else
        {
            fVolumeBGM = PlayerPrefs.GetFloat("Volume_BGMSound");
        }
        if (!PlayerPrefs.HasKey("Volume_EffectSound"))
        {
            fVolumeEffect = 0.5f;
            PlayerPrefs.SetFloat("Volume_EffectSound", 0.5f);
        }
        else
        {
            fVolumeEffect = PlayerPrefs.GetFloat("Volume_EffectSound");
        }


        StartSound();



    }



    private void StartSound()
    {
        // #. �߰��Ǵ� ���� ��� �߰��ؾ� ��
        audio_MainMenuBGM.volume = fVolumeBGM;
        audio_GameOverBGM.volume = fVolumeBGM;


        audio_AcheiveClearEffect.volume = fVolumeEffect;
        audio_AttackEffect.volume = fVolumeEffect;
        ButtonClickEffect.volume = fVolumeEffect;
        CharacterHitEffect.volume = fVolumeEffect; ;
        CharacterMoveEffect.volume = fVolumeEffect; ;
        GetCoinEffect.volume = fVolumeEffect; ;
    }



    public void OnBGM_SoundSensitivityChanged(float value) // ������� ũ�� ���� �����̴� �Լ�
    {
        fVolumeBGM = Mathf.Round(value * 100) / 100;
        PlayerPrefs.SetFloat("Volume_BGMSound", fVolumeBGM);


        // #. �߰��Ǵ� ����� �����ؾ� ��
        audio_MainMenuBGM.volume = fVolumeBGM;
        audio_GameOverBGM.volume = fVolumeBGM;
    }





    public void OnEffect_SoundSensitivityChanged(float value) // ȿ���� ũ�� ���� �����̴� �Լ�
    {
        fVolumeEffect = Mathf.Round(value * 100) / 100;
        PlayerPrefs.SetFloat("Volume_EffectSound", fVolumeEffect);


        // #. �߰��Ǵ� ����� �����ؾ� ��
        audio_AcheiveClearEffect.volume = fVolumeEffect;
        audio_AttackEffect.volume = fVolumeEffect;
        ButtonClickEffect.volume = fVolumeEffect;
        CharacterHitEffect.volume = fVolumeEffect; ;
        CharacterMoveEffect.volume = fVolumeEffect; ;
        GetCoinEffect.volume = fVolumeEffect; ;
    }
}
