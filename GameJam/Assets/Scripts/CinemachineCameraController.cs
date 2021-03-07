using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineCameraController : CinemachineExtension
{
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
    [SerializeField] private float _pan;
    [SerializeField] private float _panMultiplier = 2;
    [SerializeField] private float _tilt;
    private float _smoothedTilt;
    [SerializeField] private float _tiltsSpeed;


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
        _smoothedOffset = Vector2.Lerp(_smoothedOffset, _offset, _offsetSpeed);
        _vCamOffset.m_Offset = _smoothedOffset;

        _smoothedTilt = Mathf.Lerp(_smoothedTilt, Tilt, _tiltsSpeed);
        cinemachineRecomposer.m_Dutch = _smoothedTilt;
        cinemachineRecomposer.m_Pan = -_smoothedTilt * _panMultiplier;
    }
}

