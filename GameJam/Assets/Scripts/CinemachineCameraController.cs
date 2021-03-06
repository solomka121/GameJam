using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineCameraController : CinemachineExtension
{
    private CinemachineFreeLook _vCam;
    [SerializeField] private CinemachineCameraOffset _vCamOffset;

    [SerializeField] private float offset;
    public float Offset
    {
        get => offset;
        set
        {
            offset = value;
        }
    }
    private float smoothedOffset;
    [SerializeField] private float speed = 0.1f;
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage,
    ref CameraState state, float deltaTime)
    {
        _vCam = (CinemachineFreeLook)VirtualCamera;
    }
    private void FixedUpdate()
    {

        smoothedOffset = Mathf.Lerp(smoothedOffset, offset , speed);
        _vCamOffset.m_Offset.x = smoothedOffset;

    }
}
