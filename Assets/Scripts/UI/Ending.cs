using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace FG
{
	public class Ending : MonoBehaviour
	{
		[SerializeField] private GameObject engine;
		[SerializeField] private GameObject liquid;
		[SerializeField] private GameObject coreLight;
		[SerializeField] private GameObject engineHand;
		[SerializeField] private GameObject textPrompt;
		[SerializeField] private float timeDelay = 0f;
		[SerializeField] private GameObject sceneFade;

		private bool inArea = false;
		private bool destroyText = true;

		private void OnFire(InputValue input)
		{
			if (inArea && destroyText)
			{
				engine.SetActive(false);
				liquid.SetActive(false);
				coreLight.SetActive(false);
				engineHand.SetActive(true);
				textPrompt.SetActive(false);
				AudioManager.Instance.Play("Lever down");
				destroyText = false;
				StartCoroutine(DelayFunction());
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Ending") && destroyText)
			{
				inArea = true;
				textPrompt.SetActive(true);
			}
		}
		private void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("Ending"))
			{
				textPrompt.SetActive(false);
			}
		}

		private IEnumerator DelayFunction()
		{
			yield return new WaitForSeconds(timeDelay);
			sceneFade.transform.position = transform.position;
		}

	}
}