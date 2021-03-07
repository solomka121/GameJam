using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMultiPlayer : MonoBehaviour
{
    [SerializeField] private float _forceMultiPlayer;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody>().velocity *= _forceMultiPlayer; 
        }
    }
}
