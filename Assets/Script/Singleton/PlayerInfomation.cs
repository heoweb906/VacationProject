using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfomation : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    private static PlayerInfomation instance;

    [Header("���� ������Ʈ")]
    [SerializeField] private int goldCnt;

    public int GoldCnt { get { return goldCnt; } set { goldCnt = value; } }






    // �̱��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ
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

    // �ʿ��� �ʱ�ȭ�� �ٸ� ����� �߰��� �� �ֽ��ϴ�.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            
            // ���� ������ ���� �ҷ���


            if (!PlayerPrefs.HasKey("PlayerGold"))
            {
                Debug.Log("0������ �����Ǿ����ϴ�.");
                goldCnt = 0;
            }
            else
            {
                Debug.Log("������ �ִ� ����� �����Ǿ����ϴ�");
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
        // #. ���� �ʱ�ȭ
        if (Input.GetKeyDown(KeyCode.P))
        {
            goldCnt = 0;
            PlayerPrefs.SetInt("PlayerGold", 0);
        }

        // #. ���� �ʱ�ȭ
        if (Input.GetKeyDown(KeyCode.O))
        {
            
        }
    }








}
