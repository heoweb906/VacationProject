using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox_MagNet : MonoBehaviour
{
    // #. ȹ��Ǿ����� ȹ����� �ʾҴ��� ����
    private bool bIsGet = false;



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && !bIsGet)
            {
                bIsGet = true;

                player.ActivateForMagnet();



                Destroy(gameObject);
            }
        }
    }
}
