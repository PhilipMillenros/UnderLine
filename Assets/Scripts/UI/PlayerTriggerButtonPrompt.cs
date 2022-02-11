using System;
using UnityEngine;
using Input = FG.Input;

public class PlayerTriggerButtonPrompt : MonoBehaviour
{
	private ButtonPrompt buttonPrompt;
	private ObjectButtonPromptTriggerer objTrigg;
	[SerializeField] private GameObject detector;
	private float pickupRange;
	[SerializeField] private bool hasEel = false;

	public bool HasEel
	{
		get { return hasEel; }
		set { hasEel = value; }
	}

	private Input input;
	private ObjectButtonPromptTriggerer lastEel;
	private ObjectButtonPromptTriggerer lastSocket;

	private void Start()
	{
		buttonPrompt = FindObjectOfType<ButtonPrompt>();
		if (ReferenceEquals(buttonPrompt, null)) Debug.LogWarning("Warning: No ButtonPrompt component found. Is there a canvas in the scene?");
		pickupRange = GetComponent<Input>().PickupRange;
		//hand = GetComponent<Input>().Hand.gameObject;
		detector.GetComponent<SphereCollider>().radius = pickupRange;
		input = GetComponent<Input>();

		lastEel = null;
		lastSocket = null;
	}

	private void Update()
	{
		if (lastEel == null)
			return;

		if (input.CurrentPickup == null)
		{
			hasEel = false;
			buttonPrompt.HidePrompt(hasEel, lastEel.AssignedID);
		}
		else
		{
			hasEel = true;
			buttonPrompt.HidePrompt(hasEel, lastEel.AssignedID);
		}

		//buttonPrompt.HidePrompt(hasEel, lastEel.AssignedID);
	}

	private void OnTriggerEnter(Collider other)
	{
		//String button = other.GetComponent<ObjectButtonPromptTriggerer>().Button;

		other.TryGetComponent(out ObjectButtonPromptTriggerer objTrigg);
		if (objTrigg == null)
			return;

		String button = objTrigg.Button;

		if (ReferenceEquals(button, null))
		{
			Debug.LogWarning("Warning: No input key set for the interactable object!");
			return;
		}
		
		if(!objTrigg.NeedsEel)
			objTrigg.SwitchMaterial(true);
		else if(objTrigg.NeedsEel && hasEel)
			objTrigg.SwitchMaterial(true);
		
		//buttonPrompt.ActivateButtonPrompt(button);
		buttonPrompt.InstatiateNewPrompt(objTrigg.Button, other.transform);

		if (objTrigg.IsEel)
			lastEel = objTrigg;

		if (objTrigg.NeedsEel)
			lastSocket = objTrigg;
	}

	/*private void OnTriggerStay(Collider other)
	{
		other.TryGetComponent<ObjectButtonPromptTriggerer>(out objTrigg);
		if(ReferenceEquals(objTrigg, null))
			return;
		if(objTrigg.AssignedID < 0)
			return;

		buttonPrompt.HidePrompt(hasEel, objTrigg.AssignedID);

		if(hasEel && objTrigg.IsEel)
		{
			Debug.Log("objTrigg ID: " + objTrigg.AssignedID);
			buttonPrompt.HidePrompt(true, objTrigg.AssignedID);
			return;
		}

		if(!hasEel && objTrigg.IsEel)
		{
			buttonPrompt.HidePrompt(false, objTrigg.AssignedID);
			return;
		}
	}*/

	private void OnTriggerExit(Collider other)
	{
		DisablePrompt(other.transform);
	}

	public void DisableSocketPrompt()
	{
		DisablePrompt(lastSocket.transform);
	}

	public void DisablePrompt(Transform other)
	{
		other.TryGetComponent(out ObjectButtonPromptTriggerer objTrigg);
		if (objTrigg == null)
			return;

		objTrigg.SwitchMaterial(false);
		//buttonPrompt.DeActivateButtonPrompt();
		buttonPrompt.DeletePrompt(other.transform);
	}
}
