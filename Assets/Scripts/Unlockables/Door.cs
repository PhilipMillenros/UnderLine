using System;
using System.Collections.Generic;
using Light;
using UnityEngine;
using UnityEngine.Events;
using Activator = Activators.Activator;

[SelectionBase]
public class Door : MonoBehaviour, IUnlockable
{
	[SerializeField] private LampListener[] lightListeners;
	[SerializeField] private UnityEvent DoorOpenEvent;
	[SerializeField] private Sounds sound;
	public enum Sounds
    {
		Default,
		InnerDoor,
		OuterDoor
    }
	private Vector3 offset = new Vector3(0, 4, 0);
	private bool IsOpened;


	public void UpdateUnlockable()
	{
		TryOpening();
	}
	public void TryOpening()
	{
		if (AllLightsAreOn())
		{
			Open();
			PermanentlyActivateLights();
		}
	}

	private bool AllLightsAreOn()
	{
		for (int i = 0; i < lightListeners.Length; i++)
		{
			if (!lightListeners[i].IsActivated)
			{
				return false;
			}
		}

		return true;
	}

	public void Open()
	{
		if (IsOpened)
			return;
		for (int i = 0; i < lightListeners.Length; i++)
		{
			lightListeners[i].StopAllCoroutines();
		}
		DoorOpenEvent.Invoke();
		IsOpened = true;

		if(sound == Sounds.InnerDoor)
			FG.AudioManager.Instance.Play("ColorCodeCompleted");
		else if(sound == Sounds.OuterDoor)
			FG.AudioManager.Instance.Play("LevelCompleted");

		FG.AudioManager.Instance.Stop("Timer");
		FG.AudioManager.Instance.Play("Door Open");
	}

	private void PermanentlyActivateLights()
	{
		for (int i = 0; i < lightListeners.Length; i++)
		{
			lightListeners[i].ActivateLightPermanently();
			List<Activator> activators = lightListeners[i].activationManager.Activators[lightListeners[i].LightColor];
			for (int y = 0; y < activators.Count; y++)
			{
				if(!activators[y].IsPermanentlyActivated)
					activators[y].ActivatePermanently();
			}
		}
	}

	private void OnEnable()
	{
		UnlockableManager.Unlockables.Add(this);
	}

	private void OnDisable()
	{
		UnlockableManager.Unlockables.Remove(this);
	}
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position + offset, "door.png", true);
	}
#endif

}
