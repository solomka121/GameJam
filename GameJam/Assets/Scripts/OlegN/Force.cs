using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour
{
    [SerializeField] private float _forceForward;
    [SerializeField] private float _forceUp;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * _forceForward, ForceMode.Impulse);
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * _forceUp, ForceMode.Impulse);
        }
    }
}
