using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox_Protect : MonoBehaviour
{
    // #. 획득되었는지 획득되지 않았는제 구분
    private bool bIsGet = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && !bIsGet)
            {
                bIsGet = true;

                Debug.Log("함수 실행");
                player.ActivateProtect();

                Destroy(gameObject);
            }
        }
    }
}
