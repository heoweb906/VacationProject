using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using MoreMountains.Tools;

public class PlayerController : MonoBehaviour
{
    [Header("�̱��� / ���� ���� ���� ������Ʈ")]
    public PlayerInfomation playerInfo;
    public UIController_InGame uIController;
    public SoundManager soundManager;

    [Header("�÷��̾� �ɷ�ġ / ��ġ")]
    public int iHp;
    public float fMoveSpeed; // �÷��̾� �̵� �ӵ� 
    public float fJumpForce; // �÷��̾� ������ 
    public float fSldieForce; // �ٴ� �������� �������� �� 
    public float fDrationMagnet; // �ڼ� ������ ���� �ð�
    public float fDrationDoubleCoin; // ��� 2�� ������ ���� �ð�
    public float fDrationProtect; // ���� ������ ���ӽð�
    public float fDamageCoolTime; // ��ֹ� �浹�� ���� �ð�

    public int iNowFeverGauge;
    public int iFeverCnt;
    public float fFallForce; // �ǹ� ���� �� �������� ��



    [Header("�÷��̾� ����")]
    public bool bIsRun;
    public bool bIsJump;
    public bool bIsSlide;
    public bool bIsGround;
    public bool bIsDamage; // �������� ���� ��������, ª�� ����
    public bool bIsFever;
    public bool bIsStun;

    public bool bIsDie;
    public bool bAttackCoolTime;

    public bool bIsPaused; // �Ͻ� �������� �ƴ���

    public bool bIsMagnet;
    public bool bIsDoubleCoin;
    public bool bIsTransparent;


    [Header("������Ʈ")]
    public FollowCamera followCamera;
    public GameObject attackAreaBox_basic;
    public GameObject attackAreaBox_up;
    public GameObject attackAreaBox_down;
    public GameObject magnetRange;
    public GameObject[] feverGauge;
    public GameObject[] randomItembox;



    public GameObject[] materialTargets;
    public Material opaqueMaterial;
    public Material transparentMaterial;
    private Renderer[] rendererComponents;

    public GameObject bodyMain;
    public GameObject bodySub;


    Animator anim;
    Rigidbody rigid;


    // �����̵� �� �ݶ��̴� ũ�� ����
    public CapsuleCollider capsuleCollider;
    public float desiredSlideHeight = 1.0f; // �����̵� ���� ���� �ݶ��̴� ����
    public float desiredSlideCenterY = 0.5f; // �����̵� ���� ���� �ݶ��̴� ���� y ��ġ
    private float originalHeight; // ������ �ݶ��̴� ����
    private Vector3 originalCenter; // ������ �ݶ��̴� ����


    [Header("����� ����")]
    private float timer = 0f;
    private float interval = 1f; // 1��


    public float raycastDistance; // ��ֹ� ���� ���� �Ÿ�
    private bool isObstacleDetected = false;
    private float lastDetectionTime = 0f;
    private float ignoreDuration = 0.5f; // 0.5�� ������ ���� ����

    public int iMoveCnt;
    public int iAttackCnt;
    public int iGoldCnt;


    private void Awake()
    {
        playerInfo = FindObjectOfType<PlayerInfomation>();
        uIController = FindObjectOfType<UIController_InGame>();
        soundManager = FindObjectOfType<SoundManager>();

        // anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();


        iMoveCnt = 0;
        iAttackCnt = 0;
        iGoldCnt = 0;

        GetRendererComponents();
    }

    private void Start()
    {
        originalHeight = capsuleCollider.height;
        originalCenter = capsuleCollider.center;
    }



    void Update()
    {
        InputKey();
       
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            playerInfo.MoveDistanceRecord++;
            iMoveCnt++;
            Debug.Log("Count: " + playerInfo.MoveDistanceRecord);

            PlayerPrefs.SetFloat("MoveDistanceRecord", playerInfo.MoveDistanceRecord);

            timer = 0f;
        }

        // ������ �ð��� �����ٸ� �ٽ� ���� ���
        if (Time.time - lastDetectionTime > ignoreDuration)
        {
            isObstacleDetected = false;
        }

        SenseObstacle_Up();
        SenseObstacle_Down();


        //if (bIsRun)
        //{
        //    MoveForward();
        //}
    }



    public void FadeInPlayer()
    {
        Invoke("MainGameStart", 1.2f);
    }
    // #. ���� ���� ���� �� �����ϴ� �Լ�
    public void MainGameStart()
    {
        bIsRun = true;
        //anim.SetBool("isRun", true);
    }

    // #. ������ ����� �� �����ϴ� �Լ�
    public void MainGameEnd()
    {
        bIsRun = false;
        //anim.SetBool("isRun", false);
    }


    void InputKey()
    {
        if (bIsRun && !bIsStun)
        {
            if (Input.GetKeyDown(KeyCode.A)) MoveSideways(-4.5f);
            if (Input.GetKeyDown(KeyCode.D)) MoveSideways(4.5f);
            if (!bIsFever)
            {
                if (Input.GetKeyDown(KeyCode.W) && !bIsJump) Jump(fJumpForce);
                if (Input.GetKeyDown(KeyCode.S) && !bIsSlide) Slide();
                if (Input.GetKeyDown(KeyCode.Space) && !bAttackCoolTime) Attack();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();


    }



    // #. ������ ���ư��� �Լ�
    void MoveForward()
    {
        transform.Translate(Vector3.forward * fMoveSpeed * Time.deltaTime);
    }

    // #. �� �� �̵� �Լ�
    public void MoveSideways(float distance)
    {
        soundManager.CharacterMoveEffect.Play();

        float targetPositionX = Mathf.Clamp(transform.position.x + distance, -9.0f, 9.0f);
        Vector3 targetPosition = new Vector3(targetPositionX, transform.position.y, transform.position.z);

        // �¿� �̵��� ���� �ε巯�� �̵��� ����
        transform.DOMoveX(targetPositionX, 0.3f); // 0.3�� ���� �ε巴�� �¿� �̵�
    }
    // #. ���� �ε����� �� ������ �Լ�
    public void TouchWall(float distance)
    {
        // ��ǥ ��ġ ���
        float targetPositionX = Mathf.Clamp(-distance, -9.0f, 9.0f);
        Vector3 targetPosition = new Vector3(targetPositionX, transform.position.y, transform.position.z);

        // �¿� �̵��� ���� �ε巯�� �̵��� ����
        transform.DOMoveX(targetPositionX, 0.3f); // 0.3�� ���� �ε巴�� �¿� �̵�

        TakeDamage();
    }


    // #. ���� ���
    public void Jump(float fJumpForce_)
    {
        bIsJump = true;
        bIsSlide = false;

        rigid.velocity = Vector3.zero; // �÷��̾ �ѹ� �������������ν� ������ ������ ��

        rigid.AddForce(Vector3.up * fJumpForce_, ForceMode.Impulse);

        //anim.SetTrigger("doJump");
    }



    // #. �����̵� ���
    void Slide()
    {
        bIsSlide = true;
        bIsJump = false;
        //anim.SetTrigger("doSlide");
        rigid.AddForce(Vector3.down * fSldieForce, ForceMode.Acceleration);

        AdjustColliderSize(true);
    }
    public void EndSlide() // �ִϸ��̼� �̺�Ʈ�� �� ����
    {
        bIsSlide = false;

        AdjustColliderSize(false);
    }

    void AdjustColliderSize(bool isSliding)
    {
        if (isSliding)
        {
            // �����̵� ���� ���, �ݶ��̴� ũ�� ����
            capsuleCollider.height = desiredSlideHeight;
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, desiredSlideCenterY, capsuleCollider.center.z);
        }
        else
        {
            // �����̵� ���� ��, �ݶ��̴� ũ�� ������� ����
            capsuleCollider.height = originalHeight;
            capsuleCollider.center = originalCenter;
        }
    }



    // #. ���� ���
    void Attack()
    {
        if (!bAttackCoolTime)
        {
            soundManager.audio_AttackEffect.Play();

            StartCoroutine(ThrowRobot());
        }
    }
    IEnumerator ThrowRobot()
    {
        if (bIsJump)
        {
            bAttackCoolTime = true;
            attackAreaBox_down.SetActive(true);

            yield return new WaitForSeconds(0.3f);

            attackAreaBox_down.SetActive(false);

            yield return new WaitForSeconds(1.0f);
            bAttackCoolTime = false;
            yield break;
        }

        if (bIsSlide)
        {
            bAttackCoolTime = true;
            attackAreaBox_up.SetActive(true);

            yield return new WaitForSeconds(0.3f);

            attackAreaBox_up.SetActive(false);

            yield return new WaitForSeconds(1.0f);
            bAttackCoolTime = false;
            yield break;
        }

        bAttackCoolTime = true;
        attackAreaBox_basic.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        attackAreaBox_basic.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        bAttackCoolTime = false;
        yield break;
    }



    public void DiePlayer()
    {
        Time.timeScale = 1f;
        bIsRun = false;
        bIsDie = true;

        uIController.OnOffGameOverPanel();

        // ���� BGM�� �����ؾ� ��
        soundManager.audio_GameOverBGM.Play();

        // anim.SetBool("isRun", false);
    }



    public void TakeDamage()
    {
        if (!bIsDamage && !bIsDie)
        {
            soundManager.CharacterHitEffect.Play();

            bIsDamage = true;
            followCamera.ShakeCamera();
            iHp--;

            if (iHp <= 0)
            {
                DiePlayer();
            }
            else
            {
                StartCoroutine(DamageCooldown());
            }
        }
    }
    IEnumerator DamageCooldown()
    {
        // �ǰ� �� �Ͻ����� ���� �ð� ����
        yield return new WaitForSeconds(fDamageCoolTime);
        bIsDamage = false;
    }



    public void TogglePause()
    {
        bIsPaused = !bIsPaused;

        // #. UI�� �����״� ��
        uIController.OnOffOptionPanel();
    }



    // ��Ȧ �Ѳ��� ����� �� ����
    public void StepManHole()
    {
        StartCoroutine(PlayerDieLayer());
    }
    IEnumerator PlayerDieLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDie");
        yield return new WaitForSeconds(0.85f);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }


    // ������ ����� �� ����
    public void GetCoin()
    {
        soundManager.GetCoinEffect.Play();

        playerInfo.GoldCnt++;
        iGoldCnt++;

        if (bIsDoubleCoin) // ȹ�淮 2�� �������� ����ٸ� 2�� ȹ��
        {
            playerInfo.GoldCnt++;
            iGoldCnt++;
        }
        PlayerPrefs.SetInt("PlayerGold", playerInfo.GoldCnt);



        playerInfo.GetGoldCntRecord += 10;
        PlayerPrefs.SetInt("GetGoldCntRecord", playerInfo.GetGoldCntRecord);
        uIController.ClearAhceive_Other();
    }

    // �ǹ� ������ �ױ�
    public void GetFeverGauge()
    {
        iNowFeverGauge++;
        SeeFeverGauge();

        if (iNowFeverGauge == iFeverCnt && !bIsFever)
        {
            StartCoroutine(FeverOn());
            FeverOn();
        }
    }
    // �ǹ� ������ Ȱ��ȭ / �ð����� ȿ��
    public void SeeFeverGauge()
    {
        for (int i = 0; i < iFeverCnt; i++)
        {
            if (i < iNowFeverGauge)
            {
                feverGauge[i].SetActive(true);
            }
            else
            {
                feverGauge[i].SetActive(false);
            }

        }
    }
    // �ǹ� ���
    public IEnumerator FeverOn()
    {
        bIsFever = true;

        // �ǹ� �� ����
        rigid.AddForce(Vector3.up * fJumpForce * 6, ForceMode.Impulse);

        // �ǹ� Ÿ���� ���� ����
        CreateFeverCoin();

        while (transform.position.y < 20f)
        {
            yield return null;
        }

        // Y �� ����
        rigid.constraints |= RigidbodyConstraints.FreezePositionY;

        // �÷��̾ 5�� ���� ���� ����
        float airTime = 5f;
        float elapsedTime = 0f;

        while (elapsedTime < airTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �ǹ� Ÿ�� ���� �� ���� �ݳ�
        ReturnFeverCoins();
        bIsJump = true;

        rigid.AddForce(Vector3.down * fFallForce, ForceMode.Impulse);

        // Y �� ���� ����
        rigid.constraints &= ~RigidbodyConstraints.FreezePositionY;

        bIsFever = false;
        iNowFeverGauge = 0;
    }
    public void CreateFeverCoin()
    {
        Vector3 playerPosition = transform.position;   // �÷��̾��� ���� ��ġ
        float height = 22.0f;   // ������ ����
        float[] coinOffsets = { -4.5f, 0f, 4.5f }; // ���� ���� ��ġ �� ����
        float zInterval = 4.0f; // ���� ���� ����
        float initialZOffset = 50.0f;   // ���� ���� ��ġ
        int maxCoins = 15;  // �ִ� ���� ���� �� 

        // �ǹ� ���� ����
        for (int i = 0; i < maxCoins; i++)
        {
            for (int j = 0; j < coinOffsets.Length; j++)
            {
                // Z�� ��ġ ���
                float zPosition = playerPosition.z + initialZOffset + i * zInterval;

                // ���� ���� ��ġ
                Vector3 spawnPosition = new Vector3(coinOffsets[j], height, zPosition);

                if (i < maxCoins - 1)
                {
                    Debug.Log("���� ����");

                    GameObject feverCoin = FeverTime_PoolAssist.instance.FeverTime_PoolAssist_Pool.Get();
                    feverCoin.transform.position = spawnPosition;
                }
                else
                {
                    Debug.Log("������ ����");

                    GameObject randomItem = Instantiate(GetRandomItem(), spawnPosition, Quaternion.identity);
                }
            }
        }
    }
    private void ReturnFeverCoins()
    {
        // �ǹ� ���� �ݳ�
        GameObject[] feverCoins = GameObject.FindGameObjectsWithTag("FeverCoin");
        foreach (GameObject coin in feverCoins)
        {
            FeverTime_PoolAssist.instance.FeverTime_PoolAssist_Pool.Release(coin);
        }
    }
    // �������� �������� �����ϴ� �Լ�
    private GameObject GetRandomItem()
    {
        // randomItembox �迭�� ��� ���� ������ Ȯ��
        if (randomItembox.Length > 0)
        {
            // �迭���� �������� ���õ� ������ ��ȯ
            return randomItembox[Random.Range(0, randomItembox.Length)];
        }
        else
        {
            // �������� ���� ��� null ��ȯ
            return null;
        }
    }




    // #. ������ ���� �Լ��� 
    // 1. �ڼ� ������
    public void ActivateForMagnet()
    {
        RecordItemCnt();
        StartCoroutine(ActivateCoroutine_Magnet(fDrationMagnet));
    }
    IEnumerator ActivateCoroutine_Magnet(float duration)
    {
        bIsMagnet = true;
        magnetRange.SetActive(true);
        yield return new WaitForSeconds(duration);
        magnetRange.SetActive(false);
        bIsMagnet = false;
    }


    // 2. ��� ȹ�淮 2��
    public void ActivateForDoubleCoin()
    {
        RecordItemCnt();
        StartCoroutine(ActivateCoroutine_DoubleCoin(fDrationDoubleCoin));
    }
    IEnumerator ActivateCoroutine_DoubleCoin(float duration)
    {
        bIsDoubleCoin = true;
        yield return new WaitForSeconds(duration);
        bIsDoubleCoin = false;
    }


    // 3. ü�� ȸ��
    public void ActivateHealth()
    {
        RecordItemCnt();
        if (iHp <= 2)
        {
            iHp++;
        }
    }


    // 4. ���� ������
    public void ActivateProtect()
    {
        RecordItemCnt();

        StartCoroutine(ActivateCoroutine_Protect____(fDrationProtect));

        // StartCoroutine(ActivateCoroutine_Protect(fDrationProtect));
    }
    IEnumerator ActivateCoroutine_Protect____(float duration)
    {
        bodyMain.SetActive(false);
        bodySub.SetActive(true);
        bIsDamage = true;
        bIsTransparent = true;

        yield return new WaitForSeconds(duration);

        bodyMain.SetActive(true);
        bodySub.SetActive(false);
        bIsDamage = false;
        bIsTransparent = false;
    }

    IEnumerator ActivateCoroutine_Protect(float duration)
    {
        SetAlphaZero();
        bIsDamage = true;
        bIsTransparent = true;

        yield return new WaitForSeconds(duration);

        SetAlphaOne();
        bIsDamage = false;
        bIsTransparent = false;
    }
    // #. ���� ���׸��� ���� ����
    private void GetRendererComponents()
    {
        rendererComponents = new Renderer[materialTargets.Length];

        for (int i = 0; i < materialTargets.Length; i++)
        {
            if (materialTargets[i] != null)
            {
                Renderer renderer = materialTargets[i].GetComponent<Renderer>();

                if (renderer == null)
                {
                    Debug.LogWarning("Renderer ������Ʈ�� ã�� �� �����ϴ�.");
                }
                else
                {
                    rendererComponents[i] = renderer;
                }
            }
            else
            {
                Debug.LogWarning("Material�� ������ GameObject�� �������� �ʾҽ��ϴ�.");
            }
        }
    }
    // #. ���� ��� TransParent�� ����
    public void SetAlphaZero()
    {
        if (rendererComponents != null && transparentMaterial != null)
        {
            for (int i = 0; i < rendererComponents.Length; i++)
            {
                if (rendererComponents[i] != null)
                {
                    rendererComponents[i].material = transparentMaterial;
                }
            }
        }
        else
        {
            Debug.LogWarning("Renderer ������Ʈ �Ǵ� Transparent Material�� �������� �ʾҽ��ϴ�.");
        }

    }
    // #. ���� ��� Opaque�� ����
    public void SetAlphaOne()
    {
        if (rendererComponents != null && opaqueMaterial != null)
        {
            for (int i = 0; i < rendererComponents.Length; i++)
            {
                if (rendererComponents[i] != null)
                {
                    rendererComponents[i].material = opaqueMaterial;
                }
            }
        }
        else
        {
            Debug.LogWarning("Renderer ������Ʈ �Ǵ� Opaque Material�� �������� �ʾҽ��ϴ�.");
        }

    }

    // @. ������ ȹ�� �� ���� ��� (������)
    public void RecordItemCnt()
    {
        playerInfo.GetItemCntRecord++;
        PlayerPrefs.SetInt("GetItemCntRecord", playerInfo.GetItemCntRecord);
        uIController.ClearAhceive_Other();
    }

    // @. ���� ���� Ƚ�� ��� (������)
    public void RecordAttackCnt()
    {
        playerInfo.SuccessAttackCntRecord++;
        PlayerPrefs.SetInt("SuccessAttackCntRecord", playerInfo.SuccessAttackCntRecord);
        uIController.ClearAhceive_Other();
    }



    private void SenseObstacle_Up()
    {
        RaycastHit hit_up;
        if (Physics.Raycast(transform.position, Vector3.up, out hit_up, raycastDistance))
        {
            Debug.DrawRay(transform.position, Vector3.up * raycastDistance, Color.red);

            if (hit_up.collider.CompareTag("Obstacle") && !isObstacleDetected)
            {
                playerInfo.JumpObstacleCntRecord++;
                iAttackCnt++;
                isObstacleDetected = true;
                lastDetectionTime = Time.time;
            }
        }
    }

    private void SenseObstacle_Down()
    {
        RaycastHit hit_down;
        if (Physics.Raycast(transform.position, Vector3.down, out hit_down, raycastDistance))
        {
            Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.red);

            if (hit_down.collider.CompareTag("Obstacle") && !isObstacleDetected)
            {
                playerInfo.JumpObstacleCntRecord++;
                iAttackCnt++;
                isObstacleDetected = true;
                lastDetectionTime = Time.time;
            }
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor") && bIsJump)
        {
            bIsJump = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            bIsGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            bIsGround = false;
        }
    }



}




