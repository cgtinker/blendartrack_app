using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ArRetarget
{
	public class SelectionHelper : MonoBehaviour
	{
		public void SelectAllFiles()
		{
			SelectAllFiles(true);
		}

		public void DeselectAllFiles()
		{
			SelectAllFiles(false);
		}

		public void SelectTodaysFiles()
		{
			if (FileManager.JsonDirectories.Count == 0)
				return;

			string daytime = FileManagement.GetDateTime();
			var curTime = FileManagement.StringToInt(daytime);
			string today = FileManagement.RemoveLengthFromEnd(curTime.ToString(), 6);

			for (int i = 0; i < FileManager.JsonDirectories.Count; i++)
			{
				string day = FileManagement.RemoveLengthFromEnd(
					FileManager.JsonDirectories[i].value.ToString(), 6);

				if (today == day)
					ChangeSelectionStatus(i, true);

				else
					ChangeSelectionStatus(i, false);
			}
		}

		private static void SelectAllFiles(bool selected)
		{
			if (FileManager.JsonDirectories.Count == 0)
				return;

			for (int i = 0; i < FileManager.JsonDirectories.Count; i++)
			{
				ChangeSelectionStatus(i, selected);
			}
		}

		private static void ChangeSelectionStatus(int i, bool selected)
		{
			var btn = FileManager.JsonDirectories[i].obj.GetComponent<JsonFileButton>();
			btn.ChangeButtonStatus(selected);
		}
	}
}
