using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ArRetarget
{
    [System.Serializable]
    public class BlendShapeContainter
    {
        public List<BlendShapeData> blendShapeData;
    }

    [System.Serializable]
    public class BlendShapeData
    {
        public List<BlendShape> blendShapes;
        public int frame;
    }

    [System.Serializable]
    public class BlendShape
    {
        public float value;
        public string shapeKey;
    }
}
