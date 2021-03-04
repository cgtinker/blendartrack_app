using System.Collections;
using UnityEngine;
using TMPro;

namespace ArRetarget
{
	public class JsonPreviewHandler : MonoBehaviour
	{
		[SerializeField]
		private GameObject jsonViewerPrefab;
		[SerializeField]
		private TextMeshProUGUI jsonName;
		private GameObject jsonViewerReference;
		[SerializeField]
		private GameObject jsonButtonPointer;

		private void Start()
		{
			if (FileManager.JsonPreview.jsonFilePath != null)
			{
				StartCoroutine(OpenFile(FileManager.JsonPreview));

				return;
			}

			else if (FileManager.JsonDirectories.Count != 0)
			{
				StartCoroutine(OpenFile(FileManager.JsonDirectories[0]));

				return;
			}

			string persistentPath = Application.persistentDataPath;
			JsonDirectoryHandler handler = new JsonDirectoryHandler();
			var dirs = handler.GetDirectories(persistentPath);
			var file = dirs[0];

			if (file.jsonSize < 65)
			{
				Debug.Log(file.jsonSize + file.jsonFilePath);
				StartCoroutine(OpenFile(file));
			}
		}

		private IEnumerator OpenFile(JsonDirectory jsonDirectory)
		{
			jsonButtonPointer.GetComponent<JsonFileButton>().InitFileButton(jsonDirectory);
			this.jsonName.text = jsonDirectory.dirName;
			string contents = FileManagement.FileContents(jsonDirectory.jsonFilePath);
			yield return new WaitForEndOfFrame();

			var go = Instantiate(jsonViewerPrefab, Vector3.zero, Quaternion.identity);
			jsonViewerReference = go;
			var jsonDataImporter = go.GetComponent<JsonDataImporter>();

			jsonDataImporter.OpenFile(contents);
		}

		private void OnDisable()
		{
			Destroy(jsonViewerReference);
		}
	}
}

