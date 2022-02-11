using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class mainMenu : MonoBehaviour
{
	Resolution[] resolutions;
	public Dropdown ResolutionDropdown;
	public GameObject PlayButton;

	private void Start()
	{
		resolutions = Screen.resolutions;
		System.Array.Reverse(resolutions);
		ResolutionDropdown.ClearOptions();
		List<string> options = new List<string>();
		int currentResolutionIndex = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";
			options.Add(option);

			if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
			{
				currentResolutionIndex = i;
			}
		}
		ResolutionDropdown.AddOptions(options);
		ResolutionDropdown.value = currentResolutionIndex;
		ResolutionDropdown.RefreshShownValue();

		SelectButton(PlayButton);
	}

	public void PlayGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

	public void SelectButton(GameObject button)
	{
		if (button == null)
			return;
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(button);
	}

	public void SetFullscreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
		ResolutionDropdown.Hide();
	}

	/*public void SetSfxVolume(float sfxVolume)
	{
		AudioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
	}

	public void SetMusicVolume(float musicVolume)
	{
		AudioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
	}
	*/
}
