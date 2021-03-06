using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // тег Player повесить на шарик, для работы чекпоинтов!!!

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().CheckPoint(transform.position);
        }
    }
}
