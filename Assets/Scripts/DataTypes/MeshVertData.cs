using System.Collections.Generic;

namespace ArRetarget
{
    [System.Serializable]
    public class MeshVertData
    {
        public List<Vector> meshVerts;
        public int frame;
    }

    [System.Serializable]
    public class MeshVertDataList
    {
        public List<MeshVertData> meshVertsList;
    }
}
