using System.Collections.Generic;
using System.IO;

namespace ArRetarget
{
	public class JsonDirectoryHandler
	{
		#region get dirs
		private string[] suffixes = { "cam", "face" };

		public List<JsonDirectory> GetDirectories(string persistentPath)
		{
			List<JsonDirectory> tmp_jsonDirectories = GetValidDirectories(persistentPath);

			if (tmp_jsonDirectories.Count == 0)
				return tmp_jsonDirectories;

			for (int i = 0; i < tmp_jsonDirectories.Count; i++)
			{
				GetJsonForPreviewByName(tmp_jsonDirectories, i);
				GetFileSizes(tmp_jsonDirectories, i);
			}

			tmp_jsonDirectories.Sort((JsonDirectory x, JsonDirectory y) => y.value.CompareTo(x.value));

			return tmp_jsonDirectories;
		}

		private static void GetFileSizes(List<JsonDirectory> tmp_jsonDirectories, int i)
		{
			if (FileManagement.ValidatePath(tmp_jsonDirectories[i].jsonFilePath))
				tmp_jsonDirectories[i].jsonSize = (int)FileManagement.GetFileSize(tmp_jsonDirectories[i].jsonFilePath);
		}

		private List<JsonDirectory> GetValidDirectories(string persistentPath)
		{
			string[] dirs = FileManagement.GetDirectories(persistentPath);
			List<JsonDirectory> tmp_jsonDirectories = new List<JsonDirectory>();
			AddValidFoldersToDirectoryList(dirs, tmp_jsonDirectories);
			return tmp_jsonDirectories;
		}

		private void AddValidFoldersToDirectoryList(string[] dirs, List<JsonDirectory> tmp_jsonDirectories)
		{
			for (int t = 0; t < dirs.Length; t++)
			{
				if (!FileManagement.StringEndsWith(dirs[t], "Gallery") && !FileManagement.StringEndsWith(dirs[t], "nity") && !FileManagement.StringEndsWith(dirs[t], "il2cpp"))
				{
					AddFolderToDirectoryList(tmp_jsonDirectories, dirs, t);
				}
			}
		}

		private void AddFolderToDirectoryList(List<JsonDirectory> tmp_jsonDirectories, string[] dirs, int t)
		{
			JsonDirectory m_dir = new JsonDirectory();
			m_dir.dirName = FileManagement.GetDirectoryName(dirs[t]);
			m_dir.dirPath = dirs[t];
			tmp_jsonDirectories.Add(m_dir);
		}

		private void GetJsonForPreviewByName(List<JsonDirectory> tmp_jsonDirectories, int i)
		{
			foreach (string suffix in suffixes)
			{
				if (FileManagement.StringEndsWith(tmp_jsonDirectories[i].dirName, suffix))
				{
					//create new dir obj pointing to json and overwrite previous jsonDir
					var updatedDir = AssignJsonPathForPreview(tmp_jsonDirectories[i].dirPath, suffix, tmp_jsonDirectories[i]);
					tmp_jsonDirectories[i] = updatedDir;
				}
			}
		}
		#endregion

		#region subdir pointer to file
		//subdir
		private JsonDirectory AssignJsonPathForPreview(string path, string suffix, JsonDirectory m_dir)
		{
			if (FileManagement.ValidateDirectory(path))
			{
				return ReturnJsonWithFittingSuffix(path, suffix, m_dir);
			}

			LogManager.Instance.Log($"folder doesn't contain valid json contents <br><br>{path}", LogManager.Message.Warning);

			return m_dir;
		}

		private JsonDirectory ReturnJsonWithFittingSuffix(string path, string suffix, JsonDirectory m_dir)
		{
			m_dir.value = 0;

			FileInfo[] jsonFiles = FileManagement.GetJsonsAtPath(path);
			foreach (FileInfo json in jsonFiles)
			{
				CheckForFileSuffix(json, suffix, m_dir);
			}

			return m_dir;
		}

		private void CheckForFileSuffix(FileInfo json, string suffix, JsonDirectory m_dir)
		{
			string jsonFilename = json.Name;

			if (FileManagement.StringEndsWith(jsonFilename, suffix + ".json"))
			{
				//setting up the serializeable JsonDir obj
				m_dir.dirName = FileManagement.RemoveSuffixFromEnd(json.Name, ".json");
				m_dir.value = FileManagement.StringToInt(m_dir.dirName);
				m_dir.active = false;
				m_dir.jsonFilePath = json.FullName;
			}
		}
		#endregion
	}
}
