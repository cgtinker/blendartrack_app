using UnityEngine;
using UnityEngine.UI;

namespace ArRetarget
{
	[RequireComponent(typeof(Button))]
	public class LoadSceneOnButtonDown : MonoBehaviour
	{
		private Button button;
		[SerializeField]
		private StateMachine.State sceneToLoad;

		void Start()
		{
			button = this.gameObject.GetComponent<Button>();
			Button btn = button.GetComponent<Button>();
			btn.onClick.AddListener(TaskOnClick);
		}

		void TaskOnClick()
		{
			StateMachine.Instance.SetState(StateMachine.State.Filebrowser);
		}
	}
}
