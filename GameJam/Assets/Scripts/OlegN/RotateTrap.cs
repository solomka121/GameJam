using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrap : MonoBehaviour
{
    [SerializeField] private float _speedRotate;
    [SerializeField] private float _forceBack;
    [SerializeField] private float _forceUp;
    void Update()
    {
        transform.Rotate(Vector3.up * _speedRotate * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(-collision.gameObject.transform.forward * _forceBack, ForceMode.Impulse);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(-collision.gameObject.transform.forward * _forceUp, ForceMode.Impulse);
        }
    }
}
