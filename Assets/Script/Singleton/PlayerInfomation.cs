using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfomation : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    private static PlayerInfomation instance;

    [Header("�ΰ��� ����")]
    [SerializeField] private int goldCnt;

    // ���� �÷��̾ �����ϰ� �ִ� ����� ��
    public int GoldCnt { get { return goldCnt; } set { goldCnt = value; } }


    [Header("���� ����")]
    [SerializeField] private float moveDistanceRecord;  // �̵� �Ÿ�
    [SerializeField] private int moveDistanceRecord_Bool;  // �̵� �Ÿ�

    [SerializeField] private int getItemCntRecord;  // ȹ���� ������ ����
    [SerializeField] private int getItemCntRecord_Bool;  // ȹ���� ������ ����

    [SerializeField] private int getGoldCntRecord;  //ȹ���� ��� ����
    [SerializeField] private int getGoldCntRecord_Bool;  //ȹ���� ��� ����

    [SerializeField] private int successAttackCntRecord; // ���� ���� Ƚ��
    [SerializeField] private int successAttackCntRecord_Bool; // ���� ���� Ƚ��

    [SerializeField] private int jumpObstacleCntRecord; // �پ���� ����� ����
    [SerializeField] private int jumpObstacleCntRecord_Bool; // �پ���� ����� ����

    public float MoveDistanceRecord { get { return moveDistanceRecord; } set { moveDistanceRecord = value; } }
    public int MoveDistanceRecord_Bool { get { return moveDistanceRecord_Bool; } set { moveDistanceRecord_Bool = value; } }

    public int GetItemCntRecord { get { return getItemCntRecord; } set { getItemCntRecord = value; } }
    public int GetItemCntRecord_Bool { get { return getItemCntRecord_Bool; } set { getItemCntRecord_Bool = value; } }

    public int GetGoldCntRecord { get { return getGoldCntRecord; } set { getGoldCntRecord = value; } }
    public int GetGoldCntRecord_Bool { get { return getGoldCntRecord_Bool; } set { getGoldCntRecord_Bool = value; } }

    public int SuccessAttackCntRecord { get { return successAttackCntRecord; } set { successAttackCntRecord = value; } }
    public int SuccessAttackCntRecord_Bool { get { return successAttackCntRecord_Bool; } set { successAttackCntRecord_Bool = value; } }

    public int JumpObstacleCntRecord { get { return jumpObstacleCntRecord; } set { jumpObstacleCntRecord = value; } }
    public int JumpObstacleCntRecord_Bool { get { return jumpObstacleCntRecord_Bool; } set { jumpObstacleCntRecord_Bool = value; } }


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
            SetGameData();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    // #. ��� ���� �׽�Ʈ
    private void Update()
    {
        // #. ���� �ʱ�ȭ
        if (Input.GetKeyDown(KeyCode.P))
        {
            goldCnt = 0;
            PlayerPrefs.SetInt("PlayerGold", 0);

            moveDistanceRecord = 0f;
            moveDistanceRecord_Bool = 0;
            PlayerPrefs.SetFloat("MoveDistanceRecord", 0);
            PlayerPrefs.SetInt("MoveDistanceRecord_Bool", 0);

            getItemCntRecord = 0;
            getItemCntRecord_Bool = 0;
            PlayerPrefs.SetFloat("GetItemCntRecord", 0);
            PlayerPrefs.SetInt("GetItemCntRecord_Bool", 0);

            getGoldCntRecord = 0;
            getGoldCntRecord_Bool = 0;
            PlayerPrefs.SetFloat("GetGoldCntRecord", 0);
            PlayerPrefs.SetInt("GetGoldCntRecord_Bool", 0);

            successAttackCntRecord = 0;
            successAttackCntRecord_Bool = 0;
            PlayerPrefs.SetFloat("SuccessAttackCntRecord", 0);
            PlayerPrefs.SetInt("SuccessAttackCntRecord_Bool", 0);

            jumpObstacleCntRecord = 0;
            jumpObstacleCntRecord_Bool = 0;
            PlayerPrefs.SetFloat("JumpObstacleCntRecord", 0);
            PlayerPrefs.SetInt("JumpObstacleCntRecord_Bool", 0);

        }

        // #. ���� �ʱ�ȭ
        if (Input.GetKeyDown(KeyCode.O))
        {
            
        }
    }


    private void SetGameData()
    {
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

       
        if (!PlayerPrefs.HasKey("MoveDistanceRecord")) moveDistanceRecord = 0;
        else moveDistanceRecord = PlayerPrefs.GetFloat("MoveDistanceRecord");
        if (!PlayerPrefs.HasKey("MoveDistanceRecord_Bool")) moveDistanceRecord_Bool = 0;
        else moveDistanceRecord_Bool = PlayerPrefs.GetInt("MoveDistanceRecord_Bool");


        if (!PlayerPrefs.HasKey("GetItemCntRecord")) getItemCntRecord = 0;
        else getItemCntRecord = PlayerPrefs.GetInt("GetItemCntRecord");
        if (!PlayerPrefs.HasKey("GetItemCntRecord_Bool")) getItemCntRecord_Bool = 0;
        else getItemCntRecord_Bool = PlayerPrefs.GetInt("GetItemCntRecord_Bool");


        if (!PlayerPrefs.HasKey("GetGoldCntRecord")) getGoldCntRecord = 0;
        else getGoldCntRecord = PlayerPrefs.GetInt("GetGoldCntRecord");
        if (!PlayerPrefs.HasKey("GetGoldCntRecord_Bool")) getGoldCntRecord_Bool = 0;
        else getGoldCntRecord_Bool = PlayerPrefs.GetInt("GetGoldCntRecord_Bool");


        if (!PlayerPrefs.HasKey("SuccessAttackCntRecord")) successAttackCntRecord = 0;
        else successAttackCntRecord = PlayerPrefs.GetInt("SuccessAttackCntRecord");
        if (!PlayerPrefs.HasKey("SuccessAttackCntRecord_Bool")) successAttackCntRecord_Bool = 0;
        else successAttackCntRecord_Bool = PlayerPrefs.GetInt("SuccessAttackCntRecord_Bool");


        if (!PlayerPrefs.HasKey("JumpObstacleCntRecord")) jumpObstacleCntRecord = 0;
        else jumpObstacleCntRecord = PlayerPrefs.GetInt("JumpObstacleCntRecord");
        if (!PlayerPrefs.HasKey("JumpObstacleCntRecord_Bool")) jumpObstacleCntRecord_Bool = 0;
        else jumpObstacleCntRecord_Bool = PlayerPrefs.GetInt("JumpObstacleCntRecord_Bool");


    }
}
