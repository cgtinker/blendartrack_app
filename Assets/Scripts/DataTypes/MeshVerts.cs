using System.Collections.Generic;

namespace ArRetarget
{
    [System.Serializable]
    public class MeshVertsList
    {
        public List<MeshVerts> meshVertsList;
    }

    [System.Serializable]
    public class MeshVerts
    {
        public List<vector> meshVerts;
        public int frame;
    }
}
