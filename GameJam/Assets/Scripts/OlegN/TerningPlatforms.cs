using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerningPlatforms : MonoBehaviour
{
    [SerializeField] private float _timeWait;
    [SerializeField] private float _speedRotate;
    private float _waitTime;
    private void Update()
    {
        if (transform.rotation.z >= 0.9999f || transform.rotation.z <= -0.9999f)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            _waitTime = _timeWait;
        }
        if (_waitTime <= 0f)
        {
            transform.Rotate(Vector3.forward * _speedRotate * Time.deltaTime);
        }
        _waitTime -= Time.deltaTime;
    }
}