using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArRetarget;

public class BlendShapeImporter : MonoBehaviour, IInitViewer<BlendShapeContainter>, IUpdate<GameObject, BlendShapeContainter>
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public UpdateViewerDataHandler viewHandler;

    /// <summary>
    /// dictionary for referencing arkit shape key values
    /// might values / naming conventions will vary over time
    /// </summary>
    public Dictionary<string, int> ShapeKeyReferenceDict = new Dictionary<string, int>()
    {
        { "EyeBlinkRight", 0 },
        { "EyeWideRight", 1 },
        { "MouthLowerDownLeft", 2 },
        { "MouthRollUpper", 3 },
        { "CheekSquintLeft", 4 },
        { "MouthDimpleRight", 5 },
        { "BrowInnerUp", 6 },
        { "EyeLookInLeft", 7 },
        { "MouthPressLeft", 8 },
        { "MouthStretchRight", 9 },
        { "BrowDownLeft", 10 },
        { "MouthFunnel", 11 },
        { "NoseSneerLeft", 12 },
        { "EyeLookOutLeft", 13 },
        { "EyeLookInRight", 14 },
        { "MouthLowerDownRight", 15 },
        { "BrowOuterUpRight", 16 },
        { "MouthLeft", 17 },
        { "CheekSquintRight", 18 },
        { "JawOpen", 19 },
        { "EyeBlinkLeft", 20 },
        { "JawForward", 21 },
        { "MouthPressRight", 22 },
        { "NoseSneerRight", 23 },
        { "JawRight", 24 },
        { "MouthShrugLower", 25 },
        { "EyeSquintLeft", 26 },
        { "EyeLookOutRight", 27 },
        { "MouthFrownLeft", 28 },
        { "CheekPuff", 29 },
        { "MouthStretchLeft", 30 },
        { "TongueOut", 31 },
        { "MouthRollLower", 32 },
        { "MouthUpperUpRight", 33 },
        { "MouthShrugUpper", 34 },
        { "EyeSquintRight", 35 },
        { "EyeLookDownLeft", 36 },
        { "MouthSmileLeft", 37 },
        { "EyeWideLeft", 38 },
        { "MouthClose", 39 },
        { "JawLeft", 40 },
        { "MouthDimpleLeft", 41 },
        { "MouthFrownRight", 42 },
        { "MouthPucker", 43 },
        { "MouthRight", 44 },
        { "EyeLookUpLeft", 45 },
        { "BrowDownRight", 46 },
        { "MouthSmileRight", 47 },
        { "MouthUpperUpLeft", 48 },
        { "BrowOuterUpLeft", 49 },
        { "EyeLookUpRight", 50 },
        { "EyeLookDownRight", 51 }
    };

    private int GetBlendShapeReference(string name)
    {
        int value = ShapeKeyReferenceDict[name];
        return value;
    }

    private void SetBlendShapeReference(string name, int value)
    {
        ShapeKeyReferenceDict[name] = value;
    }

    public IEnumerator InitViewer(BlendShapeContainter data)
    {
        //generating the obj
        var m_head = Instantiate(Resources.Load("SlothHead", typeof(GameObject)) as GameObject);
        m_head.transform.parent = this.gameObject.transform;
        skinnedMeshRenderer = m_head.GetComponentInChildren<SkinnedMeshRenderer>();

        //generate the feature maps
        CreateFeatureBlendMapping();
        viewHandler.SetFrameEnd(data.blendShapeData.Count);
        yield return new WaitForEndOfFrame();

        StartCoroutine(UpdateData(m_head, data));
    }


    public IEnumerator UpdateData(GameObject obj, BlendShapeContainter data)
    {
        //blend shapes at a keyframe
        BlendShapeData m_data = data.blendShapeData[viewHandler.frame];

        for (int i = 0; i < m_data.blendShapes.Count; i++)
        {
            int mappedBlendShapeIndex = GetBlendShapeReference(m_data.blendShapes[i].shapeKey);

            if (mappedBlendShapeIndex != -1)
            {
                // mapping specific blendshape
                float blendShapeValue = m_data.blendShapes[i].value * 100;
                skinnedMeshRenderer.SetBlendShapeWeight(mappedBlendShapeIndex, blendShapeValue);
            }
        }

        yield return new WaitForEndOfFrame();

        StartCoroutine(UpdateData(obj, data));
    }

    private void CreateFeatureBlendMapping()
    {
        if (skinnedMeshRenderer == null || skinnedMeshRenderer.sharedMesh == null)
        {
            Debug.LogError("No skinned mesh renderer available to create feature blending");
            return;
        }

        const string strPrefix = "blendShape2.";
        var m_mesh = skinnedMeshRenderer.sharedMesh;

        SetBlendShapeReference("BrowDownLeft", m_mesh.GetBlendShapeIndex(strPrefix + "browDown_L"));
        SetBlendShapeReference("BrowDownRight", m_mesh.GetBlendShapeIndex(strPrefix + "browDown_R"));
        SetBlendShapeReference("BrowInnerUp", m_mesh.GetBlendShapeIndex(strPrefix + "browInnerUp"));
        SetBlendShapeReference("BrowOuterUpLeft", m_mesh.GetBlendShapeIndex(strPrefix + "browOuterUp_L"));
        SetBlendShapeReference("BrowOuterUpRight", m_mesh.GetBlendShapeIndex(strPrefix + "browOuterUp_R"));
        SetBlendShapeReference("CheekPuff", m_mesh.GetBlendShapeIndex(strPrefix + "cheekPuff"));
        SetBlendShapeReference("CheekSquintLeft", m_mesh.GetBlendShapeIndex(strPrefix + "cheekSquint_L"));
        SetBlendShapeReference("CheekSquintRight", m_mesh.GetBlendShapeIndex(strPrefix + "cheekSquint_R"));
        SetBlendShapeReference("EyeBlinkLeft", m_mesh.GetBlendShapeIndex(strPrefix + "eyeBlink_L"));
        SetBlendShapeReference("EyeBlinkRight", m_mesh.GetBlendShapeIndex(strPrefix + "eyeBlink_R"));
        SetBlendShapeReference("EyeLookDownLeft", m_mesh.GetBlendShapeIndex(strPrefix + "eyeLookDown_L"));
        SetBlendShapeReference("EyeLookDownRight", m_mesh.GetBlendShapeIndex(strPrefix + "eyeLookDown_R"));
        SetBlendShapeReference("EyeLookInLeft", m_mesh.GetBlendShapeIndex(strPrefix + "eyeLookIn_L"));
        SetBlendShapeReference("EyeLookInRight", m_mesh.GetBlendShapeIndex(strPrefix + "eyeLookIn_R"));
        SetBlendShapeReference("EyeLookOutLeft", m_mesh.GetBlendShapeIndex(strPrefix + "eyeLookOut_L"));
        SetBlendShapeReference("EyeLookOutRight", m_mesh.GetBlendShapeIndex(strPrefix + "eyeLookOut_R"));
        SetBlendShapeReference("EyeLookUpLeft", m_mesh.GetBlendShapeIndex(strPrefix + "eyeLookUp_L"));
        SetBlendShapeReference("EyeLookUpRight", m_mesh.GetBlendShapeIndex(strPrefix + "eyeLookUp_R"));
        SetBlendShapeReference("EyeSquintLeft", m_mesh.GetBlendShapeIndex(strPrefix + "eyeSquint_L"));

        //also issue
        SetBlendShapeReference("EyeSquintRight", m_mesh.GetBlendShapeIndex(strPrefix + "eyeSquint_R"));

        SetBlendShapeReference("EyeWideLeft", m_mesh.GetBlendShapeIndex(strPrefix + "eyeWide_L"));
        SetBlendShapeReference("EyeWideRight", m_mesh.GetBlendShapeIndex(strPrefix + "eyeWide_R"));
        SetBlendShapeReference("JawForward", m_mesh.GetBlendShapeIndex(strPrefix + "jawForward"));
        SetBlendShapeReference("JawLeft", m_mesh.GetBlendShapeIndex(strPrefix + "jawLeft"));
        SetBlendShapeReference("JawOpen", m_mesh.GetBlendShapeIndex(strPrefix + "jawOpen"));
        SetBlendShapeReference("JawRight", m_mesh.GetBlendShapeIndex(strPrefix + "jawRight"));
        SetBlendShapeReference("MouthClose", m_mesh.GetBlendShapeIndex(strPrefix + "mouthClose"));
        SetBlendShapeReference("MouthDimpleLeft", m_mesh.GetBlendShapeIndex(strPrefix + "mouthDimple_L"));
        SetBlendShapeReference("MouthDimpleRight", m_mesh.GetBlendShapeIndex(strPrefix + "mouthDimple_R"));
        SetBlendShapeReference("MouthFrownLeft", m_mesh.GetBlendShapeIndex(strPrefix + "mouthFrown_L"));
        SetBlendShapeReference("MouthFrownRight", m_mesh.GetBlendShapeIndex(strPrefix + "mouthFrown_R"));
        SetBlendShapeReference("MouthFunnel", m_mesh.GetBlendShapeIndex(strPrefix + "mouthFunnel"));
        SetBlendShapeReference("MouthLeft", m_mesh.GetBlendShapeIndex(strPrefix + "mouthLeft"));
        SetBlendShapeReference("MouthLowerDownLeft", m_mesh.GetBlendShapeIndex(strPrefix + "mouthLowerDown_L"));
        SetBlendShapeReference("MouthLowerDownRight", m_mesh.GetBlendShapeIndex(strPrefix + "mouthLowerDown_R"));
        SetBlendShapeReference("MouthPressLeft", m_mesh.GetBlendShapeIndex(strPrefix + "mouthPress_L"));
        SetBlendShapeReference("MouthPressRight", m_mesh.GetBlendShapeIndex(strPrefix + "mouthPress_R"));
        SetBlendShapeReference("MouthPucker", m_mesh.GetBlendShapeIndex(strPrefix + "mouthPucker"));
        SetBlendShapeReference("MouthRight", m_mesh.GetBlendShapeIndex(strPrefix + "mouthRight"));
        SetBlendShapeReference("MouthRollLower", m_mesh.GetBlendShapeIndex(strPrefix + "mouthRollLower"));
        SetBlendShapeReference("MouthRollUpper", m_mesh.GetBlendShapeIndex(strPrefix + "mouthRollUpper"));
        SetBlendShapeReference("MouthShrugLower", m_mesh.GetBlendShapeIndex(strPrefix + "mouthShrugLower"));
        SetBlendShapeReference("MouthShrugUpper", m_mesh.GetBlendShapeIndex(strPrefix + "mouthShrugUpper"));
        SetBlendShapeReference("MouthSmileLeft", m_mesh.GetBlendShapeIndex(strPrefix + "mouthSmile_L"));
        SetBlendShapeReference("MouthSmileRight", m_mesh.GetBlendShapeIndex(strPrefix + "mouthSmile_R"));
        SetBlendShapeReference("MouthStretchLeft", m_mesh.GetBlendShapeIndex(strPrefix + "mouthStretch_L"));
        SetBlendShapeReference("MouthStretchRight", m_mesh.GetBlendShapeIndex(strPrefix + "mouthStretch_R"));
        SetBlendShapeReference("MouthUpperUpLeft", m_mesh.GetBlendShapeIndex(strPrefix + "mouthUpperUp_L"));
        SetBlendShapeReference("MouthUpperUpRight", m_mesh.GetBlendShapeIndex(strPrefix + "mouthUpperUp_R"));
        SetBlendShapeReference("NoseSneerLeft", m_mesh.GetBlendShapeIndex(strPrefix + "noseSneer_L"));
        SetBlendShapeReference("NoseSneerRight", m_mesh.GetBlendShapeIndex(strPrefix + "noseSneer_R"));

        //issue:
        SetBlendShapeReference("TongueOut", m_mesh.GetBlendShapeIndex(strPrefix + "tongueOut"));
    }
}
