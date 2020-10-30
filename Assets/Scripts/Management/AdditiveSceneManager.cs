using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneManager : MonoBehaviour
{
    public int currentScene = 0;

    public static Dictionary<int, string> AndroidScenes = new Dictionary<int, string>
    {
        { 0, "Tutorial"},
        { 1, "PoseDataTracker" },
        { 2, "FaceMeshTracker" }
    };

    public static Dictionary<int, string> IOSScenes = new Dictionary<int, string>
    {
        { 0, "Tutorial"},
        { 1, "PoseDataTracker" },
        { 2, "ShapeKeyTracker" }
    };

    public static Dictionary<int, string> RemoteScenes = new Dictionary<int, string>
    {
        { 0, "Tutorial"},
        { 1, "PoseDataTracker" },
        { 2, "ShapeKeyTracker" }
    };

    public Dictionary<int, string> GetDeviceScenes()
    {
        switch (DeviceManager.Instance.device)
        {
            case DeviceManager.Device.Android:
                return AndroidScenes;

            case DeviceManager.Device.iOS:
                return IOSScenes;

            case DeviceManager.Device.Remote:
                return RemoteScenes;

            default:
                return AndroidScenes;
        }
    }

    public void SwitchScene(int sceneIndex)
    {
        Dictionary<int, string> tmp = GetDeviceScenes();
    }

    public void AddScene(int sceneIndex)
    {

    }

    public void UnloadScene(int sceneIndex)
    {

    }

    public void ReloadCurrentScene(int sceneIndex)
    {
        SceneManager.LoadSceneAsync("");
    }





    /*

    private void RestartSession()
    {
        Debug.Log("Restarting Session");

        string scene = null;
        switch (DeviceManager.Instance.device)
        {
            case DeviceManager.Device.Android:
                switch (DeviceManager.Instance.Ability)
                {
                    case DeviceManager.TrackingType.ArCore_CameraPose:
                        scene = ArCore_CameraPose;
                        break;

                    case DeviceManager.TrackingType.ArCore_FaceMesh:
                        scene = ArCore_FaceMesh;
                        break;
                }
                break;

            case DeviceManager.Device.iOs:
                switch (DeviceManager.Instance.Ability)
                {
                    case DeviceManager.TrackingType.ArKit_CameraPose:
                        scene = ArKit_CameraPose;
                        break;

                    case DeviceManager.TrackingType.ArKit_BlendShapes:
                        scene = ArKit_ShapeKeys;
                        break;
                }
                break;

            case DeviceManager.Device.Remote:
                switch (DeviceManager.Instance.Ability)
                {
                    case DeviceManager.TrackingType.Remote_CameraPose:
                        scene = ArCore_CameraPose;
                        break;

                    case DeviceManager.TrackingType.Remote_FaceMesh:
                        scene = ArCore_FaceMesh;
                        break;

                    case DeviceManager.TrackingType.Remote_FaceKeys:
                        scene = ArKit_ShapeKeys;
                        break;
                }
                break;
        }


        if (scene != null)
            SceneManager.LoadSceneAsync(scene);
        else
            Debug.Log("Load main menu");
        }
    */
}
