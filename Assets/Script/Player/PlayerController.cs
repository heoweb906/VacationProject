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
    [Header("싱글톤 / 게임 진행 관련 오브젝트")]
    public PlayerInfomation playerInfo;
    public UIController_InGame uIController;
    public SoundManager soundManager;

    [Header("플레이어 능력치 / 수치")]
    public int iHp;
    public float fMoveSpeed; // 플레이어 이동 속도 
    public float fJumpForce; // 플레이어 점프력 
    public float fSldieForce; // 바닥 방향으로 가해지는 힘 
    public float fDrationMagnet; // 자석 아이템 지속 시간
    public float fDrationDoubleCoin; // 골드 2배 아이템 지속 시간
    public float fDrationProtect; // 무적 아이템 지속시간
    public float fDamageCoolTime; // 장애물 충돌시 무적 시간

    public int iNowFeverGauge;
    public int iFeverCnt;
    public float fFallForce; // 피버 끝난 뒤 떨어지는 힘



    [Header("플레이어 상태")]
    public bool bIsRun;
    public bool bIsJump;
    public bool bIsSlide;
    public bool bIsGround;
    public bool bIsDamage; // 데미지를 받은 상태인지, 짧은 무적
    public bool bIsFever;
    public bool bIsStun;

    public bool bIsDie;
    public bool bAttackCoolTime;

    public bool bIsPaused; // 일시 정지인지 아닌지

    public bool bIsMagnet;
    public bool bIsDoubleCoin;
    public bool bIsTransparent;


    [Header("오브젝트")]
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


    // 슬라이딩 시 콜라이더 크기 변경
    public CapsuleCollider capsuleCollider;
    public float desiredSlideHeight = 1.0f; // 슬라이드 중일 때의 콜라이더 높이
    public float desiredSlideCenterY = 0.5f; // 슬라이드 중일 때의 콜라이더 센터 y 위치
    private float originalHeight; // 원래의 콜라이더 높이
    private Vector3 originalCenter; // 원래의 콜라이더 센터


    [Header("잡다한 변수")]
    private float timer = 0f;
    private float interval = 1f; // 1초


    public float raycastDistance; // 장애물 감지 레이 거리
    private bool isObstacleDetected = false;
    private float lastDetectionTime = 0f;
    private float ignoreDuration = 0.5f; // 0.5초 동안은 감지 무시

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

        // 무시할 시간이 지났다면 다시 감지 허용
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
    // #. 게임 최초 실행 시 동작하는 함수
    public void MainGameStart()
    {
        bIsRun = true;
        //anim.SetBool("isRun", true);
    }

    // #. 게임이 종료될 때 동작하는 함수
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



    // #. 앞으로 나아가는 함수
    void MoveForward()
    {
        transform.Translate(Vector3.forward * fMoveSpeed * Time.deltaTime);
    }

    // #. 양 옆 이동 함수
    public void MoveSideways(float distance)
    {
        soundManager.CharacterMoveEffect.Play();

        float targetPositionX = Mathf.Clamp(transform.position.x + distance, -9.0f, 9.0f);
        Vector3 targetPosition = new Vector3(targetPositionX, transform.position.y, transform.position.z);

        // 좌우 이동에 대한 부드러운 이동만 적용
        transform.DOMoveX(targetPositionX, 0.3f); // 0.3초 동안 부드럽게 좌우 이동
    }
    // #. 벽에 부딪혔을 때 실행할 함수
    public void TouchWall(float distance)
    {
        // 목표 위치 계산
        float targetPositionX = Mathf.Clamp(-distance, -9.0f, 9.0f);
        Vector3 targetPosition = new Vector3(targetPositionX, transform.position.y, transform.position.z);

        // 좌우 이동에 대한 부드러운 이동만 적용
        transform.DOMoveX(targetPositionX, 0.3f); // 0.3초 동안 부드럽게 좌우 이동

        TakeDamage();
    }


    // #. 점프 기능
    public void Jump(float fJumpForce_)
    {
        bIsJump = true;
        bIsSlide = false;

        rigid.velocity = Vector3.zero; // 플레이어를 한번 정지시켜줌으로써 일정한 점프를 함

        rigid.AddForce(Vector3.up * fJumpForce_, ForceMode.Impulse);

        //anim.SetTrigger("doJump");
    }



    // #. 슬라이딩 기능
    void Slide()
    {
        bIsSlide = true;
        bIsJump = false;
        //anim.SetTrigger("doSlide");
        rigid.AddForce(Vector3.down * fSldieForce, ForceMode.Acceleration);

        AdjustColliderSize(true);
    }
    public void EndSlide() // 애니메이션 이벤트에 들어가 있음
    {
        bIsSlide = false;

        AdjustColliderSize(false);
    }

    void AdjustColliderSize(bool isSliding)
    {
        if (isSliding)
        {
            // 슬라이딩 중인 경우, 콜라이더 크기 조절
            capsuleCollider.height = desiredSlideHeight;
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, desiredSlideCenterY, capsuleCollider.center.z);
        }
        else
        {
            // 슬라이딩 종료 시, 콜라이더 크기 원래대로 복구
            capsuleCollider.height = originalHeight;
            capsuleCollider.center = originalCenter;
        }
    }



    // #. 공격 기능
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

        // 메인 BGM도 종료해야 함
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
        // 피격 후 일시적인 무적 시간 설정
        yield return new WaitForSeconds(fDamageCoolTime);
        bIsDamage = false;
    }



    public void TogglePause()
    {
        bIsPaused = !bIsPaused;

        // #. UI를 껐다켰다 함
        uIController.OnOffOptionPanel();
    }



    // 맨홀 뚜껑을 밟았을 때 실행
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


    // 코인을 얻었을 때 실행
    public void GetCoin()
    {
        soundManager.GetCoinEffect.Play();

        playerInfo.GoldCnt++;
        iGoldCnt++;

        if (bIsDoubleCoin) // 획득량 2배 아이템을 얻었다면 2배 획득
        {
            playerInfo.GoldCnt++;
            iGoldCnt++;
        }
        PlayerPrefs.SetInt("PlayerGold", playerInfo.GoldCnt);



        playerInfo.GetGoldCntRecord += 10;
        PlayerPrefs.SetInt("GetGoldCntRecord", playerInfo.GetGoldCntRecord);
        uIController.ClearAhceive_Other();
    }

    // 피버 게이지 쌓기
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
    // 피버 게이지 활성화 / 시각적인 효과
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
    // 피버 기능
    public IEnumerator FeverOn()
    {
        bIsFever = true;

        // 피버 중 점프
        rigid.AddForce(Vector3.up * fJumpForce * 6, ForceMode.Impulse);

        // 피버 타임의 코인 생성
        CreateFeverCoin();

        while (transform.position.y < 20f)
        {
            yield return null;
        }

        // Y 축 고정
        rigid.constraints |= RigidbodyConstraints.FreezePositionY;

        // 플레이어가 5초 동안 높이 유지
        float airTime = 5f;
        float elapsedTime = 0f;

        while (elapsedTime < airTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 피버 타임 종료 후 코인 반납
        ReturnFeverCoins();
        bIsJump = true;

        rigid.AddForce(Vector3.down * fFallForce, ForceMode.Impulse);

        // Y 축 고정 해제
        rigid.constraints &= ~RigidbodyConstraints.FreezePositionY;

        bIsFever = false;
        iNowFeverGauge = 0;
    }
    public void CreateFeverCoin()
    {
        Vector3 playerPosition = transform.position;   // 플레이어의 현재 위치
        float height = 22.0f;   // 코인의 높이
        float[] coinOffsets = { -4.5f, 0f, 4.5f }; // 코인 생성 위치 및 간격
        float zInterval = 4.0f; // 코인 생성 간격
        float initialZOffset = 50.0f;   // 최초 생성 위치
        int maxCoins = 15;  // 최대 생성 코인 수 

        // 피버 코인 생성
        for (int i = 0; i < maxCoins; i++)
        {
            for (int j = 0; j < coinOffsets.Length; j++)
            {
                // Z축 위치 계산
                float zPosition = playerPosition.z + initialZOffset + i * zInterval;

                // 실제 생성 위치
                Vector3 spawnPosition = new Vector3(coinOffsets[j], height, zPosition);

                if (i < maxCoins - 1)
                {
                    Debug.Log("코인 생성");

                    GameObject feverCoin = FeverTime_PoolAssist.instance.FeverTime_PoolAssist_Pool.Get();
                    feverCoin.transform.position = spawnPosition;
                }
                else
                {
                    Debug.Log("아이템 생성");

                    GameObject randomItem = Instantiate(GetRandomItem(), spawnPosition, Quaternion.identity);
                }
            }
        }
    }
    private void ReturnFeverCoins()
    {
        // 피버 코인 반납
        GameObject[] feverCoins = GameObject.FindGameObjectsWithTag("FeverCoin");
        foreach (GameObject coin in feverCoins)
        {
            FeverTime_PoolAssist.instance.FeverTime_PoolAssist_Pool.Release(coin);
        }
    }
    // 랜덤으로 아이템을 선택하는 함수
    private GameObject GetRandomItem()
    {
        // randomItembox 배열이 비어 있지 않은지 확인
        if (randomItembox.Length > 0)
        {
            // 배열에서 랜덤으로 선택된 아이템 반환
            return randomItembox[Random.Range(0, randomItembox.Length)];
        }
        else
        {
            // 아이템이 없을 경우 null 반환
            return null;
        }
    }




    // #. 아이템 관련 함수들 
    // 1. 자석 아이템
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


    // 2. 골드 획득량 2배
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


    // 3. 체력 회복
    public void ActivateHealth()
    {
        RecordItemCnt();
        if (iHp <= 2)
        {
            iHp++;
        }
    }


    // 4. 무적 아이템
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
    // #. 렌더 머테리얼 정보 추출
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
                    Debug.LogWarning("Renderer 컴포넌트를 찾을 수 없습니다.");
                }
                else
                {
                    rendererComponents[i] = renderer;
                }
            }
            else
            {
                Debug.LogWarning("Material을 적용할 GameObject가 지정되지 않았습니다.");
            }
        }
    }
    // #. 렌더 방식 TransParent로 변경
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
            Debug.LogWarning("Renderer 컴포넌트 또는 Transparent Material이 지정되지 않았습니다.");
        }

    }
    // #. 렌더 방식 Opaque로 변경
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
            Debug.LogWarning("Renderer 컴포넌트 또는 Opaque Material이 지정되지 않았습니다.");
        }

    }

    // @. 아이템 획득 총 개수 상승 (업적용)
    public void RecordItemCnt()
    {
        playerInfo.GetItemCntRecord++;
        PlayerPrefs.SetInt("GetItemCntRecord", playerInfo.GetItemCntRecord);
        uIController.ClearAhceive_Other();
    }

    // @. 공격 성공 횟수 상승 (업적용)
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




