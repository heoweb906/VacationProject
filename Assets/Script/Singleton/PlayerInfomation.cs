using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfomation : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static PlayerInfomation instance;

    [Header("방학 프로젝트")]
    [SerializeField] private int goldCnt;

    public int GoldCnt { get { return goldCnt; } set { goldCnt = value; } }






    // 싱글톤 인스턴스에 접근할 수 있는 프로퍼티
    public static PlayerInfomation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerInfomation>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject("PlayerInformation");
                    instance = singleton.AddComponent<PlayerInfomation>();
                }
            }
            return instance;
        }
    }

    // 필요한 초기화나 다른 기능을 추가할 수 있습니다.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            
            // 최초 생성시 정보 불러옴


            if (!PlayerPrefs.HasKey("PlayerGold"))
            {
                Debug.Log("0원으로 설정되었습니다.");
                goldCnt = 0;
            }
            else
            {
                Debug.Log("기존에 있던 값대로 설정되었습니다");
                goldCnt = PlayerPrefs.GetInt("PlayerGold");
            }

           





            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }










    private void Update()
    {
        // #. 코인 초기화
        if (Input.GetKeyDown(KeyCode.P))
        {
            goldCnt = 0;
            PlayerPrefs.SetInt("PlayerGold", 0);
        }

        // #. 업적 초기화
        if (Input.GetKeyDown(KeyCode.O))
        {
            
        }
    }








}
