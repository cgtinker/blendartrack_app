using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;
using System;

namespace ArRetarget
{

	public class PointCloudHandler : MonoBehaviour, IInit<string, string>, IStop, IPrefix
	{
		private ARPointCloud arPointCloud;
		private List<Vector3> points;
		private string filePath;
		private bool lastFrame;
		private bool recording;
		private float pointDensity;

		#region init
		public void Init(string path, string title)
		{
			pointDensity = PlayerPrefs.GetFloat("pointDensity", 0.75f);
			filePath = $"{path}{title}_{j_Prefix()}.json";
			lastFrame = false;
			JsonFileWriter.WriteDataToFile(path: filePath, text: "", title: "points", lastFrame: false);
			arPointCloud = GameObject.FindGameObjectWithTag("pointCloud").GetComponent<ARPointCloud>();
			ReceivePointCloud();
			recording = true;
		}
		#endregion

		#region referencing point cloud
		public void ReceivePointCloud()
		{
			arPointCloud.updated += OnPointCloudChanged;
		}

		public void StopTracking()
		{
			Debug.Log("Stopped PC Tracking");
			lastFrame = true;
		}

		private void OnPointCloudChanged(ARPointCloudUpdatedEventArgs eventArgs)
		{
			if (!lastFrame)
				GetCurrentPoints();

			else
			{
				Debug.Log("unsub from pc update");
				arPointCloud.updated -= OnPointCloudChanged;
				GetCurrentPoints();
			}
		}
		#endregion

		#region getting and writing data
		//used percent = value between 0.01f & 1.00f
		private int GetStep(int arrayLength, float usedPercent)
		{
			int step;
			float arrayPart = arrayLength * usedPercent;

			if (arrayPart > 1)
			{
				step = Mathf.FloorToInt(arrayLength / arrayPart);
				return step;
			}

			return arrayLength;
		}

		bool write;
		int curTick;
		static string contents;
		public void GetCurrentPoints()
		{
			if (!recording)
				return;

			NativeSlice<Vector3> pointCloud = (NativeSlice<Vector3>)arPointCloud.positions;

			int step = GetStep(pointCloud.Length, pointDensity);
			SetupSerializableData(pointCloud, step);
			WriteDataToFile();
		}

		private void WriteDataToFile()
		{
			if (write && !lastFrame)
			{
				JsonFileWriter.WriteDataToFile(
					path: filePath, text: contents, title: "", lastFrame: lastFrame);
				contents = "";
			}

			if (write && lastFrame)
			{
				JsonFileWriter.WriteDataToFile(
					path: filePath, text: contents, title: "", lastFrame: lastFrame);
				contents = "";

				recording = false;
				filePath = null;
			}
		}

		private void SetupSerializableData(NativeSlice<Vector3> pointCloud, int step)
		{
			string json = "";

			for (int i = 0; i < pointCloud.Length - step; i += step)
			{
				json += JsonUtility.ToJson(pointCloud[i]) + ",";
			}

			json += JsonUtility.ToJson(pointCloud[pointCloud.Length]);
			(contents, curTick, write) = DataHelper.JsonContentTicker(
					lastFrame: lastFrame, curTick: curTick, reqTick: 65, contents: contents, json: json);
		}

		public string j_Prefix()
		{
			return "cloud";
		}
		#endregion
	}
}