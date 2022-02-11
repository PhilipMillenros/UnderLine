using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class pauseManager : MonoBehaviour
{

	public bool paused = false;

	public GameObject PauseMenu;
	public GameObject ResumeButton;

	GP2 action;

	private void Awake()
	{
		action = new GP2();
	}

	private void OnEnable()
	{
		action.Enable();
	}
	private void OnDisable()
	{
		action.Disable();
	}

	private void Start()
	{
		action.Player.Esc.performed += _ => DeterminePause();
	}

	private void DeterminePause()
	{
		if (paused)
			ResumeGame();
		else
			PauseGame();
	}


	public void PauseGame()
	{
		Time.timeScale = 0;
		AudioListener.pause = true;
		paused = true;
		PauseMenu.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		SelectButton(ResumeButton);
	}

	public void ResumeGame()
	{
		Time.timeScale = 1;
		AudioListener.pause = false;
		paused = false;
		PauseMenu.SetActive(false);
	}

	public void SelectButton(GameObject button)
	{
		if (button == null)
			return;
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(button);
	}

	public void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
		Debug.Log("Quitting.");
	}

	public void LoadMenu()
	{
		ResumeGame();
		SceneManager.LoadScene(0);
	}
}
