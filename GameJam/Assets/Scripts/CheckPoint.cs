using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private float _deathYlevel;
    //this method creates checkpoints
    // tag Player on player for script to work!!
    //CHECK POINTS MUST NOT BE PLACED IN INVERSED GRAVITY ZONES
    //CHECK POINTS MUST BE REWRITTEN ON RESPAWN
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<Player>().CurrentCheckPoint != transform.position)
            {
                other.gameObject.GetComponent<Player>().CheckPoint(transform.position);
                other.gameObject.GetComponent<Player>().PlayerDeathLevelChecking(transform.position.y - _deathYlevel);
                Debug.Log("CheckPoint Rewritten to current spot!");
            }  
            if(/*other.gameObject.GetComponent<ChangeGravityTest>().enabled == true && */other.gameObject.GetComponent<ChangeGravityTest>().CurrentCheckPoint != transform.position)
            {
                other.gameObject.GetComponent<ChangeGravityTest>().CheckPoint(transform.position);
                other.gameObject.GetComponent<ChangeGravityTest>().PlayerDeathLevelChecking(transform.position.y - _deathYlevel);
                Debug.Log("CheckPoint Rewritten to current spot!");
            }
        }
    }
}
