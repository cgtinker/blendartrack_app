using UnityEngine;
using ArRetarget;

public class DataManager : MonoBehaviour
{
    JsonSerializer jsonSerializer;

    private CameraPoseHandler cameraPoseHandler;
    private FaceMeshHandler faceMeshHandler;

    private string attachmentPath;
    private bool recording;
    private int frame = 0;

    void Start()
    {
        attachmentPath = Application.persistentDataPath + "/temp.json";
        recording = false;
        jsonSerializer = this.gameObject.GetComponent<JsonSerializer>();
        Debug.Log("Session started");
    }

    public void AssignDataType()
    {

        switch (DeviceManager.Instance.DataType)
        {
            case DeviceManager.RecData.ArCore_CameraPose:
                cameraPoseHandler = GameObject.FindGameObjectWithTag("retarget").GetComponent<CameraPoseHandler>();
                Debug.Log("Assigned ArCore Camera Pose Method");
                break;

            case DeviceManager.RecData.ArCore_FaceMesh:
                faceMeshHandler = GameObject.FindGameObjectWithTag("retarget").GetComponent<FaceMeshHandler>();
                Debug.Log("Assigned ArCore Face Mesh Method");
                break;

            case DeviceManager.RecData.ArKit_CameraPose:
                Debug.Log("No Method assigned");
                break;

            case DeviceManager.RecData.ArKit_ShapeKeys:
                Debug.Log("No Method assigned");
                break;
        }
    }

    public void ToggleRecording()
    {
        if (!recording)
            InitRetargeting();

        recording = !recording;

        Debug.Log($"Recording: {recording}");
    }

    private void InitRetargeting()
    {
        switch (DeviceManager.Instance.DataType)
        {
            case DeviceManager.RecData.ArCore_CameraPose:
                cameraPoseHandler.InitCamera();
                break;
            case DeviceManager.RecData.ArCore_FaceMesh:
                faceMeshHandler.InitFaceMesh();
                break;
            case DeviceManager.RecData.ArKit_CameraPose:
                break;
            case DeviceManager.RecData.ArKit_ShapeKeys:
                break;
        }

        Debug.Log("Init Retargeting");
    }

    void Update()
    {
        if (recording)
        {
            frame++;

            switch (DeviceManager.Instance.DataType)
            {
                case DeviceManager.RecData.ArCore_CameraPose:
                    cameraPoseHandler.GetCameraPoseData(frame);
                    break;

                case DeviceManager.RecData.ArCore_FaceMesh:
                    faceMeshHandler.ProcessMeshVerts(frame);
                    break;

                case DeviceManager.RecData.ArKit_CameraPose:
                    break;
                case DeviceManager.RecData.ArKit_ShapeKeys:
                    break;
            }
        }
    }

    public void SerializeJson()
    {
        DeleteFile.FileAtMediaPath(attachmentPath);
        Debug.Log("Serializing json");

        switch (DeviceManager.Instance.DataType)
        {
            case DeviceManager.RecData.ArCore_CameraPose:
                CameraPoseDataList cpd = new CameraPoseDataList()
                {
                    poseList = cameraPoseHandler.cameraDataList
                };

                jsonSerializer.SerializeCameraPoseData(cpd, attachmentPath);
                break;

            case DeviceManager.RecData.ArCore_FaceMesh:
                MeshVertDataList mvd = new MeshVertDataList()
                {
                    meshVertsList = faceMeshHandler.meshVertsList
                };

                jsonSerializer.SerializeMeshData(mvd, attachmentPath);
                break;

            case DeviceManager.RecData.ArKit_CameraPose:
                break;
            case DeviceManager.RecData.ArKit_ShapeKeys:
                break;
        }
    }
}