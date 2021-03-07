using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineCameraController : CinemachineExtension
{
    [Header("Settings")]
    private CinemachineFreeLook _vCam;
    [SerializeField] private CinemachineCameraOffset _vCamOffset;
    [SerializeField] private CinemachineRecomposer cinemachineRecomposer;

    public Vector2 Offset
    {
        get => _offset;
        set
        {
            _offset = value;
        }
    }
    [Header("Offset")]
    [SerializeField] private Vector2 _offset;
    private Vector2 _smoothedOffset;
    [SerializeField] private float _offsetSpeed = 0.1f;

    public float Tilt
    {
        get => _tilt;
        set
        {
            _tilt = value;
        }
    }
    [Header("Tilt")]
    [SerializeField] private float _pan;
    [SerializeField] private float _panMultiplier = 2;
    [SerializeField] private float _tilt;
    private float _smoothedTilt;
    [SerializeField] private float _tiltsSpeed;

    [Header("Reversed Gravity")]
    // "++" - Must be changed when Reverse off / on
    public bool IsReversed; // "++"
    public float _reverseGravityAngle = 180; // "++"
    private float _smoothedReverseGravityAngle;
    [SerializeField] private float _reverseGravityTurnSpeed = 0.02f;
    public Vector2 _reverseOffset; // "++"
    private Vector2 _smoothReverseOffset;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage,
    ref CameraState state, float deltaTime)
    {
        _vCam = (CinemachineFreeLook)VirtualCamera;
    }

    private void Start()
    {
        cinemachineRecomposer = GetComponent<CinemachineRecomposer>();
        _vCam.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
    }

    private void FixedUpdate()
    {
        if (IsReversed)
        {
            _smoothReverseOffset = Vector2.Lerp(_smoothReverseOffset, _offset + _reverseOffset, _offsetSpeed);
            _vCamOffset.m_Offset = _smoothReverseOffset;

            _smoothedTilt = Mathf.Lerp(_smoothedTilt, Tilt, _tiltsSpeed);
            cinemachineRecomposer.m_Dutch = -_smoothedTilt + _reverseGravityAngle;
            cinemachineRecomposer.m_Pan = _smoothedTilt * _panMultiplier;

            _smoothedReverseGravityAngle = Mathf.Lerp(_smoothedReverseGravityAngle, _reverseGravityAngle, _reverseGravityTurnSpeed);
            cinemachineRecomposer.m_Dutch = _smoothedReverseGravityAngle;
        }
        else
        {
            // turn out of reverse;
            if (_reverseGravityAngle == 0 && _smoothedReverseGravityAngle >= 0.4f)
            {
                print("reverse");
                _smoothedReverseGravityAngle = Mathf.Lerp(_smoothedReverseGravityAngle, 0, _reverseGravityTurnSpeed);
                cinemachineRecomposer.m_Dutch = _smoothedReverseGravityAngle;
            }
            else
            {
                _smoothedReverseGravityAngle = 0;
            }
            if (_smoothReverseOffset.y <= _offset.y && _reverseOffset == Vector2.zero)
            {
                _smoothReverseOffset = Vector2.Lerp(_smoothReverseOffset, _offset, _offsetSpeed);
            }
            //

            _smoothedOffset = Vector2.Lerp(_smoothedOffset, _offset, _offsetSpeed);
            _vCamOffset.m_Offset = _smoothedOffset + _smoothReverseOffset;

            _smoothedTilt = Mathf.Lerp(_smoothedTilt, Tilt, _tiltsSpeed);
            cinemachineRecomposer.m_Dutch = _smoothedTilt + _smoothedReverseGravityAngle;
            cinemachineRecomposer.m_Pan = -_smoothedTilt * _panMultiplier;
        }


    }
    public void InvertInputs(bool state)
    {
        _vCam.m_YAxis.m_InvertInput = !state;
        _vCam.m_XAxis.m_InvertInput = state;
    }
}

