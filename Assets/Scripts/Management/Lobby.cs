using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public string ArCore_CameraPose = "CameraTracker";
    public string ArCore_FaceMesh = "FaceTracker";
    public string ArKit_CameraPose = "CameraTracker";
    public string ArKit_ShapeKeys = "Sample Scene";

    public void LoadCameraPoseScene()
    {
        SceneManager.LoadSceneAsync(ArCore_CameraPose);
    }

    public void LoadFaceMeshScene()
    {
        SceneManager.LoadSceneAsync(ArCore_FaceMesh);
    }
}
