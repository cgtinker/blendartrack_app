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
        Application.onBeforeRender += PreRenderEvent;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= PreRenderEvent;
    }

    void PreRenderEvent()
    {
        if (_myCamera == null || sourceCamera == null)
            return;

        _myCamera.projectionMatrix = sourceCamera.projectionMatrix;
    }
}
