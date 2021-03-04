using UnityEngine;

namespace ArRetarget
{
	public class AnchorHandler : MonoBehaviour, IInit<string, string>, IPrefix, IStop
	{
		ReferenceCreator referenceCreator;

		string filePath;
		public void Init(string path, string title)
		{
			filePath = $"{path}{title}_{j_Prefix()}.json";
			JsonFileWriter.WriteDataToFile(path: filePath, text: "", title: "anchorData", lastFrame: false);
			referenceCreator = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ReferenceCreator>();
		}

		public string j_Prefix()
		{
			return "anchor";
		}

		static string jsonContents;
		public void StopTracking()
		{
			for (int i = 0; i < referenceCreator.anchors.Count; i++)
			{
				var vector = referenceCreator.anchors[i].transform.position;
				string json = JsonUtility.ToJson(vector);

				if (i < referenceCreator.anchors.Count - 1)
					jsonContents += $"{json},";
				else
					jsonContents += json;
			}

			//TODO: Check if data gets closed
			jsonContents += "]}";
			JsonFileWriter.WriteDataToFile(path: filePath, text: jsonContents, title: "", lastFrame: true);
			jsonContents = "";
			filePath = null;
		}
	}
}
