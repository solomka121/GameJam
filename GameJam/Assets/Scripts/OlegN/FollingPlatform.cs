using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollingPlatform : MonoBehaviour
{
    [SerializeField] private float _timeCrash;
    [SerializeField] private float _timeSpawn;

    private Rigidbody _rb;
    private Vector3 _startTransform;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
    }

    private void Start()
    {
        _startTransform = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke(nameof(CrashMethod), _timeCrash);
        }
    }

    private void CrashMethod()
    {
        _rb.isKinematic = false;
        Invoke(nameof(SpawnMethod), _timeSpawn);
    }

    private void SpawnMethod()
    {
        _rb.isKinematic = true;
        transform.position = _startTransform;
    }
}