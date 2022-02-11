using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextLevelTriggerManager : MonoBehaviour
{
	public Animator animator;

	public void OnTriggerEnter()
	{
		animator.SetTrigger("FadeOut");
	}

	public void OnFadeComplete()
	{
		if (SceneManager.sceneCountInBuildSettings <= SceneManager.GetActiveScene().buildIndex + 1)
		{
			SceneManager.LoadScene(0);
		}
		else
		{
			FindObjectOfType<nextLevelManager>().LoadNextScene();
		}
	}
}