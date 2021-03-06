using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private float _deathYlevel;
    //this method creates checkpoints
    // тег Player повесить на шарик, для работы чекпоинтов!!!
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<Player>().CurrentCheckPoint != transform.position)
            {
                other.gameObject.GetComponent<Player>().CheckPoint(transform.position);
                other.gameObject.GetComponent<Player>().PlayerDeathLevelChecking(transform.position.y - _deathYlevel);
            }
        }
    }
}
