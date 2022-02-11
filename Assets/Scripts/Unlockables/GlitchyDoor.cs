using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchyDoor : MonoBehaviour
{
	public bool Open = false;
	public bool IsActive = false;

	public void OpenDoor()
	{
		Open = true;
		IsActive = true;
	}

	public void CloseDoor()
	{
		Open = false;
	}

	public void OpenSound()
	{
		FG.AudioManager.Instance.Play("Glitchy Doors Open");
	}
	public void CloseSound()
	{
		FG.AudioManager.Instance.Play("Glitchy Doors Close");
	}
}
