using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderPass : MonoBehaviour
{

    // #. Fog����
    public float fFogStartNum; // ���� ���� �Ÿ�
    public float fFogEndNum; //  ���� �� �Ÿ�
    public float fFadeDuration;


    // ���׸� ��Ȱ��ȭ�ϴ� �Լ�
    public void DisableFog()
    {
        RenderSettings.fog = false;
    }




    public void StartPointReached()
    {
        Debug.Log("��ŸƮ ����Ʈ �۵��߽��ϴ�.");

        float targetFogEndDistance = fFogStartNum;
       
        float startFogEndDistance = RenderSettings.fogEndDistance;

        // DOVirtual.Float�� ����Ͽ� ���� ����
        DOTween.To(() => startFogEndDistance, x => startFogEndDistance = x, targetFogEndDistance, fFadeDuration)
            .OnUpdate(() => RenderSettings.fogEndDistance = startFogEndDistance)
            .SetEase(Ease.Linear);
    }



    public void EndPointReached()
    {
        Debug.Log("���� ����Ʈ �۵��߽��ϴ�.");

        float targetFogEndDistance = fFogEndNum;
        float startFogEndDistance = RenderSettings.fogEndDistance;

        // DOVirtual.Float�� ����Ͽ� ���� ����
        DOTween.To(() => startFogEndDistance, x => startFogEndDistance = x, targetFogEndDistance, fFadeDuration)
            .OnUpdate(() => RenderSettings.fogEndDistance = startFogEndDistance)
            .SetEase(Ease.Linear);
    }

}