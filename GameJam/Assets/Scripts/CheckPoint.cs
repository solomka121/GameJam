using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private float _deathYlevel;
    [SerializeField] private float _offsetY; // Чтобы игрок спавнился выше,чем чекпоинт,когда это нужно
    //this method creates checkpoints
    // тег Player повесить на шарик, для работы чекпоинтов!!!

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<Player>().CurrentCheckPoint != transform.position)
            {
                Vector3 checkPoint = transform.position;
                checkPoint.y += _offsetY;
                other.gameObject.GetComponent<Player>().CheckPoint(checkPoint);
                other.gameObject.GetComponent<Player>().PlayerDeathLevelChecking(transform.position.y - _deathYlevel);
            }
        }
    }
}
