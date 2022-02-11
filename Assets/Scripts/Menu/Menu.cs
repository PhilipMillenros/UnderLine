using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FG
{
	public class Menu : MonoBehaviour
	{
		public void LoadSceneByIndex(int index)
		{
			SceneManager.LoadScene(index);
		}

		public void Quitclick()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
			Debug.Log("Quitting.");
		}
	}
}