using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public SerializeJson jsonSerializer;

    private CameraPoseHandler cameraPoseHandler;
    private FaceMeshHandler faceMeshHandler;

    private string attachmentPath;
    private bool recording;
    private int frame = 0;

    public enum RecData
    {
        ArCore_CameraPose,
        ArKit_CameraPose,
        ArCore_FaceMesh,
        ArKit_ShapeKeys
    };

    public RecData dataType
    {
        get;
        set;
    }

    private void Start()
    {
        attachmentPath = Application.persistentDataPath + "/temp.json";
        recording = false;

        Debug.Log("Session started");
    }

    public void SetDataType(RecData data)
    {
        dataType = data;

        switch (dataType)
        {
            case RecData.ArCore_CameraPose:
                cameraPoseHandler = GameObject.FindGameObjectWithTag("retarget").GetComponent<CameraPoseHandler>();
                Debug.Log("Assigned ArCore Camera Pose Method");
                break;

            case RecData.ArCore_FaceMesh:
                faceMeshHandler = GameObject.FindGameObjectWithTag("retarget").GetComponent<FaceMeshHandler>();
                Debug.Log("Assigned ArCore Face Mesh Method");
                break;

            case RecData.ArKit_CameraPose:
                Debug.Log("No Method assigned");
                break;

            case RecData.ArKit_ShapeKeys:
                Debug.Log("No Method assigned");
                break;
        }
    }

    public void ToggleRecording()
    {
        if (!recording)
            InitRetargeting();

        recording = !recording;
    }

    private void InitRetargeting()
    {
        switch (dataType)
        {
            case RecData.ArCore_CameraPose:
                cameraPoseHandler.InitCameraData();
                break;
            case RecData.ArCore_FaceMesh:
                faceMeshHandler.InitFaceMesh();
                break;
            case RecData.ArKit_CameraPose:
                break;
            case RecData.ArKit_ShapeKeys:
                break;
        }
    }

    private void Update()
    {
        if (recording)
        {
            frame++;
            switch (dataType)
            {
                case RecData.ArCore_CameraPose:
                    cameraPoseHandler.SetCameraData(frame);
                    break;

                case RecData.ArCore_FaceMesh:
                    faceMeshHandler.ProcessMeshVerts(frame);
                    break;

                case RecData.ArKit_CameraPose:
                    break;
                case RecData.ArKit_ShapeKeys:
                    break;
            }
        }
    }

    public void SerializeJson()
    {
        switch (dataType)
        {
            case RecData.ArCore_CameraPose:
                jsonSerializer.SerializeCameraPoseData(cameraPoseHandler.cameraDataList, attachmentPath);
                break;

            case RecData.ArCore_FaceMesh:
                jsonSerializer.SerializeMeshData(faceMeshHandler.meshVertsList, attachmentPath);
                break;

            case RecData.ArKit_CameraPose:
                break;
            case RecData.ArKit_ShapeKeys:
                break;
        }
    }
}