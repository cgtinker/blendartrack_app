using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
	public static class FileManager
	{
		private static List<JsonDirectory> recentJsonDirectories = new List<JsonDirectory>();

		private static bool instantPreview = false;
		private static List<JsonDirectory> jsonDirectories = new List<JsonDirectory>();
		private static JsonDirectory jsonPreview = new JsonDirectory();

		public static JsonDirectory JsonPreview
		{
			get { return jsonPreview; }
			set { jsonPreview = value; }
		}

		public static List<JsonDirectory> JsonDirectories
		{
			get { return jsonDirectories; }
			set
			{
				jsonDirectories = value;
				recentJsonDirectories = value;
			}
		}

		public static List<JsonDirectory> GetRecentDirectories
		{
			get { return recentJsonDirectories; }
		}

		public static bool InstantPreview
		{
			get { return instantPreview; }
			set { instantPreview = value; }
		}

		public static void ChangeDirectoryProperties(JsonDirectory jsonDirectory)
		{
			jsonDirectories[jsonDirectory.index] = jsonDirectory;
		}
	}
}

