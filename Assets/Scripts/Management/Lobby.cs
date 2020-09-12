using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public static string ArCore_CameraPose = "CameraTracker";
    public static string ArCore_FaceMesh = "FaceTracker";
    public static string ArKit_CameraPose = "CameraTracker";
    public static string ArKit_ShapeKeys = "Sample Scene";

    public void LoadCameraPoseScene()
    {
        SceneManager.LoadSceneAsync(ArCore_CameraPose);
    }

    public void LoadFaceMeshScene()
    {
        SceneManager.LoadSceneAsync(ArCore_FaceMesh);
    }
}
