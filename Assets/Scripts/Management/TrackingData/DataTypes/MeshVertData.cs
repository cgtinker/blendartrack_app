using System.Collections.Generic;

namespace ArRetarget
{
    /// <summary>
    /// descripes a list of vertecies from a mesh at a given frame
    /// </summary>
    [System.Serializable]
    public class MeshData
    {
        /// <summary>
        /// list of vertecies from a mesh
        /// </summary>
        public List<Vector> pos;
        /// <summary>
        /// frame or index
        /// </summary>
        public int frame;
    }

    /// <summary>
    /// moving mesh vertecies container for json serialization
    /// </summary>
    [System.Serializable]
    public class MeshDataContainer
    {
        public List<MeshData> meshDataList;
    }
}
