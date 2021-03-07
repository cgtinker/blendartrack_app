using UnityEngine;
using UnityEngine.UI;

namespace ArRetarget
{
	[RequireComponent(typeof(Button))]
	public class DisableLogOnButtonDown : MonoBehaviour
	{
		private Button button;

		void Start()
		{
			button = this.gameObject.GetComponent<Button>();
			Button btn = button.GetComponent<Button>();
			btn.onClick.AddListener(TaskOnClick);
		}

		void TaskOnClick()
		{
			LogManager.Instance.Log("", LogManager.Message.Disable);
		}
	}
}
