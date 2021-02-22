using System;
using UnityEngine;

namespace ArRetarget
{
    [System.Serializable]
    public class JsonDirectory
    {
        /// <summary>
        /// path to the directory
        /// </summary>
        public string dirPath;
        /// <summary>
        /// name of the directory
        /// </summary>
        public string dirName;
        /// <summary>
        /// path to the json file
        /// </summary>
        public string jsonFilePath;
        /// <summary>
        /// instantiated button object
        /// </summary>
        public GameObject obj;
        /// <summary>
        /// bool to determine if toggle has been selected
        /// </summary>
        public bool active;
        /// <summary>
        /// index for reference
        /// </summary>
        public int index;
        /// <summary>
        /// value for sorting
        /// </summary>
        public Int64 value;
    }
}
