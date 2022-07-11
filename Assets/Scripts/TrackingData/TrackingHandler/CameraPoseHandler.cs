﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace ArRetarget
{
	public class CameraPoseHandler : MonoBehaviour, IInit<string, string>, IGet<int, bool>, IPrefix
	{
		private GameObject mainCamera;

		private int curTick;
		private static string jsonContents;
		private bool write;

		private void Start()
		{
			var dataManager = GameObject.FindGameObjectWithTag(TagManager.TrackingDataManager).GetComponent<TrackingDataManager>();
			dataManager.SetRecorderReference(this.gameObject);
		}

		private string filePath;
		public void Init(string path, string title)
		{
			filePath = $"{path}{title}_{j_Prefix()}.json";
			JsonFileWriter.WriteDataToFile(path: filePath, text: "", title: "cameraPoseList", lastFrame: false);
			mainCamera = GameObject.FindGameObjectWithTag(TagManager.MainCamera);
		}

		//get data at a specific frame
		public void GetFrameData(int frame, bool lastFrame)
		{
			if (mainCamera)
				AccessingPoseData(frame, lastFrame);
		}

		private void AccessingPoseData(int frame, bool lastFrame)
		{
			//getting vertex data
			var poseData = DataHelper.GetPoseData(mainCamera, frame);
			string json = JsonUtility.ToJson(poseData);
			(jsonContents, curTick, write) = DataHelper.JsonContentTicker(lastFrame: lastFrame, curTick: curTick, reqTick: 61, contents: jsonContents, json: json);

			if (write)
			{
				JsonFileWriter.WriteDataToFile(path: filePath, text: jsonContents, title: "", lastFrame: lastFrame);
				jsonContents = "";
				if (lastFrame)
					filePath = null;
			}
		}

		//json file prefix
		public string j_Prefix()
		{
			return "cam";
		}
	}
}
