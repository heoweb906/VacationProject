using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public UIController_InGame uiInGame;

    public Transform position_startPoint;


    private void Awake()
    {
        uiInGame = FindObjectOfType<UIController_InGame>();
        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        PlayerPositionReset();
    }



    public void PlayerPositionReset()
    {
        if (player != null && position_startPoint != null)
        {
            player.transform.position = position_startPoint.position;
            player.transform.rotation = position_startPoint.rotation;
        }
        else
        {
            Debug.LogError("PlayerController 또는 start 위치가 지정되지 않았습니다!");
        }
    }


    public void PlayerReset()
    {
        PlayerPositionReset();

        Time.timeScale = 1f; 

        player.bIsJump = false;
        player.bIsSlide = false;
        player.bIsDamage = false; ;
        player.bIsDie = false; 
        player.bAttackCoolTime = false;
        player.bIsPaused = false;
        player.bIsMagnet = false;
        player.bIsDoubleCoin = false;

        player.MainGameEnd();
        player.SetAlphaOne();
    }




}

