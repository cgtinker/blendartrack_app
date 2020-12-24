using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://forum.unity.com/threads/sync-multiple-cameras-with-arfoundation.718844/
[ExecuteAlways]
public class XRCameraMatcher : MonoBehaviour
{
    public Camera sourceCamera;

    private Camera _myCamera;

    private void OnEnable()
    {
        _myCamera = GetComponent<Camera>();
    }

    private void OnPreRender()
    {
        if (_myCamera == null)
        {
            return;
        }
        if (sourceCamera == null)
        {
            return;
        }

        _myCamera.nearClipPlane = sourceCamera.nearClipPlane;

        _myCamera.farClipPlane = sourceCamera.farClipPlane;

        _myCamera.orthographic = sourceCamera.orthographic;
        _myCamera.orthographicSize = sourceCamera.orthographicSize;
        _myCamera.fieldOfView = sourceCamera.fieldOfView;

        _myCamera.cullingMatrix = sourceCamera.cullingMatrix;
        _myCamera.nonJitteredProjectionMatrix = sourceCamera.nonJitteredProjectionMatrix;
        _myCamera.projectionMatrix = sourceCamera.projectionMatrix;
        _myCamera.useJitteredProjectionMatrixForTransparentRendering = sourceCamera.useJitteredProjectionMatrixForTransparentRendering;
        _myCamera.worldToCameraMatrix = sourceCamera.worldToCameraMatrix;

        _myCamera.stereoConvergence = sourceCamera.stereoConvergence;
        _myCamera.stereoSeparation = sourceCamera.stereoSeparation;
        _myCamera.stereoTargetEye = sourceCamera.stereoTargetEye;

        _myCamera.sensorSize = sourceCamera.sensorSize;
        _myCamera.gateFit = sourceCamera.gateFit;
    }
}
