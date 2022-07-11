using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;

namespace ArRetarget
{
	public class CameraIntrinsicsHandler : MonoBehaviour, IInit<string, string>, IPrefix, IGet<int, bool> //,IJson
	{
		private Camera arCamera;
		private ARCameraManager arCameraManager;

		private string filePath;

		private int curTick;
		private static string contents;
		private bool write;

		#region initializing
		public void Init(string path, string title)
		{
			filePath = $"{path}{title}_{j_Prefix()}.json";
			JsonFileWriter.WriteDataToFile(path: filePath, text: "", title: "cameraProjection", lastFrame: false);

			//reference to the ar camera
			if (arCameraManager == null || arCamera == null)
			{
				var obj = GameObject.FindGameObjectWithTag(TagManager.ARSessionOrigin);
				arCameraManager = obj.GetComponentInChildren<ARCameraManager>();
				arCamera = GameObject.FindGameObjectWithTag(TagManager.MainCamera).GetComponent<Camera>();
			}

			curTick = 0;
			contents = "";
			write = false;
		}
		#endregion

		#region getting and writing data
		public void GetFrameData(int frame, bool lastFrame)
		{
			if (arCamera == null)
			{
				arCamera = GameObject.FindGameObjectWithTag(TagManager.MainCamera).GetComponent<Camera>();
				return;
			}

			if (!lastFrame)
			{
				WriteDataContinously(frame, lastFrame);
			}

			else if (lastFrame)
			{
				WriteDataLastFrame(frame, lastFrame);
			}
		}

		private void WriteDataContinously(int frame, bool lastFrame)
		{
			//proj matrix
			Matrix4x4 m_matrix = arCamera.projectionMatrix;
			CameraProjectionMatrix tmp = new CameraProjectionMatrix();
			tmp.frame = frame;
			tmp.cameraProjectionMatrix = m_matrix;

			//prepare data
			string json = JsonUtility.ToJson(tmp);
			(contents, curTick, write) = DataHelper.JsonContentTicker(lastFrame: lastFrame, curTick: curTick, reqTick: 23, contents: contents, json: json);

			//write contents
			if (write)
			{
				JsonFileWriter.WriteDataToFile(path: filePath, text: contents, "", lastFrame: lastFrame);
				contents = "";
			}

			if (lastFrame)
				filePath = null;
		}

		private void WriteDataLastFrame(int frame, bool lastFrame)
		{
			//proj matrix
			Matrix4x4 m_matrix = arCamera.projectionMatrix;
			CameraProjectionMatrix tmp = new CameraProjectionMatrix();
			tmp.frame = frame;
			tmp.cameraProjectionMatrix = m_matrix;

			//config
			CameraConfig m_config = GetCameraConfiguration();
			ScreenResolution m_res = GetResolution();

			//prepare data
			string matrix = JsonUtility.ToJson(tmp);
			string config = JsonUtility.ToJson(m_config);
			string res = JsonUtility.ToJson(m_res);

			//phrasing
			string par = "}";
			string quote = "\"";
			string json = $"{matrix}],{quote}cameraConfig{quote}:{config},{quote}resolution{quote}:{res}{par}";

			//writing
			contents += json;
			JsonFileWriter.WriteDataToFile(path: filePath, text: contents, "", lastFrame: lastFrame);
			contents = "";
		}

		public string j_Prefix()
		{
			return "intrinsics";
		}
		#endregion

		#region prepare subsystem data
		//formatting subsystem camera config for json conversion
		private CameraConfig GetCameraConfiguration()
		{
			CameraConfig config = new CameraConfig();
			NativeArray<XRCameraConfiguration> xrCameraConfig = GetXRCameraConfigurations();

			config.fps = (int)xrCameraConfig[0].framerate;
			config.height = xrCameraConfig[0].height;
			config.width = xrCameraConfig[0].width;

			return config;
		}

		//formatting screen resolution for json conversion
		private ScreenResolution GetResolution()
		{
			ScreenResolution res = new ScreenResolution();

			Vector2 m_res = GetScreenResolution();
			res.screenWidth = m_res.x;
			res.screenHeight = m_res.y;

			return res;
		}

		//accessing the screen resolution
		private Vector2 GetScreenResolution()
		{
			Vector2 screenResolution = new Vector2(Screen.width, Screen.height);
			return screenResolution;
		}
		#endregion

		#region receive subsystem data
		//accessing subsystem camera intrinsics data
		private XRCameraIntrinsics GetCameraIntrinsics()
		{
			XRCameraIntrinsics intrinsics;
			arCameraManager.TryGetIntrinsics(out intrinsics);

			return intrinsics;
		}

		//accessing subsystem camera config data
		private NativeArray<XRCameraConfiguration> GetXRCameraConfigurations()
		{
			NativeArray<XRCameraConfiguration> xrCameraConfigurations;
			xrCameraConfigurations = arCameraManager.GetConfigurations(allocator: Unity.Collections.Allocator.Temp);

			return xrCameraConfigurations;
		}
		#endregion
	}
}