using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderPass : MonoBehaviour
{

    // #. Fog관련
    public float fFogStartNum; // 포그 시작 거리
    public float fFogEndNum; //  포그 끝 거리
    public float fFadeDuration;


    // 포그를 비활성화하는 함수
    public void DisableFog()
    {
        RenderSettings.fog = false;
    }




    public void StartPointReached()
    {
        Debug.Log("스타트 포인트 작동했습니다.");

        float targetFogEndDistance = fFogStartNum;
       
        float startFogEndDistance = RenderSettings.fogEndDistance;

        // DOVirtual.Float을 사용하여 값을 조정
        DOTween.To(() => startFogEndDistance, x => startFogEndDistance = x, targetFogEndDistance, fFadeDuration)
            .OnUpdate(() => RenderSettings.fogEndDistance = startFogEndDistance)
            .SetEase(Ease.Linear);
    }



    public void EndPointReached()
    {
        Debug.Log("엔드 포인트 작동했습니다.");

        float targetFogEndDistance = fFogEndNum;
        float startFogEndDistance = RenderSettings.fogEndDistance;

        // DOVirtual.Float을 사용하여 값을 조정
        DOTween.To(() => startFogEndDistance, x => startFogEndDistance = x, targetFogEndDistance, fFadeDuration)
            .OnUpdate(() => RenderSettings.fogEndDistance = startFogEndDistance)
            .SetEase(Ease.Linear);
    }

}