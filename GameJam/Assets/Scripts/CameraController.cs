using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraAnchor;
    [Header("Follow")]
    [SerializeField] private Transform _followTarget;
    [SerializeField] private float _speed = 0.1f;
    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 5;
    private float _cameraAnchorCurrentXrotation;
    [SerializeField] private int _minXlook;
    [SerializeField] private int _maxXlook;
    [Header("Shake")]
    [SerializeField] private bool shake;
    private Vector3 _smoothedTargetPosition;
    private Vector3 _offset;

    [Header("WallRun")]
    //      WALL RUN
    public bool isWallRunning;
    public float cameraTiltZ;

    private float cameraTilt;
    //

    private void Start()
    {
        //Cursor.visible = false;

        //Using lookAt without camera anchor
        _offset = transform.position - _followTarget.position;
    }

    private void LateUpdate()
    {

        #region Mouse Rotate

        _cameraAnchor.transform.eulerAngles += Vector3.up * Input.GetAxis("Mouse X") * _rotationSpeed;
        _cameraAnchorCurrentXrotation += Input.GetAxis("Mouse Y") * _rotationSpeed;
        _cameraAnchorCurrentXrotation = Mathf.Clamp(_cameraAnchorCurrentXrotation, _minXlook, _maxXlook);

        Vector3 clampedCameraAnchorAngle = _cameraAnchor.eulerAngles;
        clampedCameraAnchorAngle.x = -_cameraAnchorCurrentXrotation; // "-" inverted inputs

        _cameraAnchor.eulerAngles = clampedCameraAnchorAngle;

        // Using lookAt without camera anchor
        //Quaternion camTurnAnlge = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);
        //_offset = camTurnAnlge * _offset;

        #endregion

        #region Position Change

        _smoothedTargetPosition = Vector3.Slerp(_smoothedTargetPosition, _followTarget.position, _speed);
        _cameraAnchor.position = _smoothedTargetPosition;

        // Using lookAt without camera anchor
        _smoothedTargetPosition = Vector3.Slerp(_smoothedTargetPosition, _followTarget.position, _speed);
        transform.position = _smoothedTargetPosition + _offset;
        transform.LookAt(_followTarget.position);

        #endregion

    }

    private void FixedUpdate()
    {

        if (shake)
        {
            shake = false;
            StartCoroutine(CameraShake(0.3f, 0.06f));
        }

        #region Wall Running

        /*if (isWallRunning)
        {
            cameraTilt = Mathf.Lerp(cameraTilt, cameraTiltZ , 0.1f);
            transform.rotation = Quaternion.Euler(new Vector3(cameraRotation.x, cameraRotation.y, cameraRotation.z + cameraTilt));
        }
        else
        {
            cameraTilt = Mathf.Lerp(cameraTilt, cameraRotation.z, 0.1f);
            transform.rotation = Quaternion.Euler(new Vector3(cameraRotation.x, cameraRotation.y, cameraRotation.z + cameraTilt));
        }*/

        #endregion
    }

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 startPosition = transform.position;

        float timeByPass = 0.0f;

        while (timeByPass < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            transform.position = new Vector3(startPosition.x + x, startPosition.y + y, startPosition.z);

            timeByPass += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = startPosition;
        StopCoroutine(CameraShake(0, 0));
    }

    public void ChangeFollowTarget(Transform target)
    {
        _followTarget = target;
    }
}