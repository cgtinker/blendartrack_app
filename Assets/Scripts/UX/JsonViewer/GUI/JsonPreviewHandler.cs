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
			if (FileManager.JsonPreview.jsonSize < 65 && FileManager.JsonPreview.jsonFilePath != null)
			{
				StartCoroutine(OpenFile(FileManager.JsonPreview));
			}

			else if (FileManager.JsonPreview.jsonSize > 65)
			{
				LogManager.Instance.Log("The recording size is to large for the in app viewer. But you can still import the data in blender!", LogManager.Message.Warning);
			}

			else
			{
				LogManager.Instance.Log("The filepath seems to be corupted. Try to import it in Blender, if that doesn't work drop me a message.", LogManager.Message.Error);
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

