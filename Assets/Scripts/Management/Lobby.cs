using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public static string[] AndroidScenes =
    {
        "PoseDataTracker",
        "FaceMeshTracker",

    };
    public static string[] IOSScenes =
    {
        "PoseDataTracker",
        "ShapeKeyTracker"
    };

    public static string ArCore_CameraPose = "PoseDataTracker";
    public static string ArCore_FaceMesh = "FaceMeshTracker";
    public static string ArKit_CameraPose = "PoseDataTracker";
    public static string ArKit_ShapeKeys = "ShapeKeyTracker";

    public void LoadCameraPoseScene()
    {
        SceneManager.LoadSceneAsync(ArCore_CameraPose);
    }

    public void LoadFaceMeshScene()
    {
        SceneManager.LoadSceneAsync(ArKit_ShapeKeys);
    }
}
