#if (UNITY_EDITOR)
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainSceneInEditor : MonoBehaviour
{
	// Start is called before the first frame update
	void Awake()
	{
		Scene main = SceneManager.GetSceneByName("Main");

		if (!main.isLoaded)
		{
			SceneManager.LoadScene("Main", LoadSceneMode.Additive);
		}
	}
}
#endif