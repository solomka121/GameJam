using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().Respawn();
        }
    }
}
