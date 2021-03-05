using UnityEngine;
using System.Collections.Generic;
using TMPro;
namespace ArRetarget
{
	[RequireComponent(typeof(FilebrowserManager),
		(typeof(SelectionHelper)),
		(typeof(ShareAndDeleteJsonDirectories)))]
	public class FilebrowserEventListener : MonoBehaviour
	{
		[SerializeField]
		private GameObject loadingScreenObj;
		[SerializeField]
		private TextMeshProUGUI loadingScreenText;

		private FilebrowserManager filebrowserManager;
		private ShareAndDeleteJsonDirectories shareAndDelete;

		[SerializeField]
		private List<TextMeshProUGUI> selectionHelpers = new List<TextMeshProUGUI>();
		private SelectionHelper selectionHelper;

		private void Start()
		{
			filebrowserManager = this.gameObject.GetComponent<FilebrowserManager>();
			shareAndDelete = this.gameObject.GetComponent<ShareAndDeleteJsonDirectories>();
			selectionHelper = this.gameObject.GetComponent<SelectionHelper>();
			loadingScreenObj.SetActive(false);
		}

		#region Share and Delete
		public void OnShareFiles()
		{
			shareAndDelete.OnShareSelectedFiles(loadingScreenObj, loadingScreenText, FileManager.JsonDirectories);
		}

		public void OnDeleteFiles()
		{
			shareAndDelete.OnDeleteSelectedFiles(FileManager.JsonDirectories);
			PurgeOrphans.DestroyOrphanButtons(FileManager.JsonDirectories);
			filebrowserManager.Start();
		}
		#endregion

		#region File Selection
		private enum SelectionState
		{
			today,
			none,
			all,
			custom
		}

		private SelectionState selectionState;
		private SelectionState setSelectionState
		{
			get { return selectionState; }
			set
			{
				selectionState = value;
				UpdateSelectionState();
			}
		}

		public void OnSelectFile()
		{
			setSelectionState = SelectionState.custom;
		}

		public void OnSelectAllFiles()
		{
			setSelectionState = SelectionState.all;
			selectionHelper.SelectAllFiles();
		}

		public void OnSelectNoneFiles()
		{
			setSelectionState = SelectionState.none;
			selectionHelper.DeselectAllFiles();
		}

		public void OnSelectTodayFiles()
		{
			setSelectionState = SelectionState.today;
			selectionHelper.SelectTodaysFiles();
		}

		private void UpdateSelectionState()
		{
			PurgeOrphans.PurgeOrphanZips();
			foreach (TextMeshProUGUI text in selectionHelpers)
			{
				if (text.fontStyle != FontStyles.Normal)
					text.fontStyle = FontStyles.Normal;
			}

			switch (selectionState)
			{
				case SelectionState.none:
				selectionHelpers[2].fontStyle = FontStyles.Underline | FontStyles.Bold;
				break;
				case SelectionState.all:
				selectionHelpers[1].fontStyle = FontStyles.Underline | FontStyles.Bold;
				break;
				case SelectionState.today:
				selectionHelpers[0].fontStyle = FontStyles.Underline | FontStyles.Bold;
				break;
				case SelectionState.custom:
				break;
			}
		}
		#endregion


	}
}
