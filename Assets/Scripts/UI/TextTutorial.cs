using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextTutorial : MonoBehaviour
{
    public GameObject UIObject;
    // public Animator animator;


    private void Start()
    {
        UIObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)

    {
        if (other.CompareTag("Player"))
        {
            UIObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // animator.SetTrigger("TextFadeOut");
            UIObject.SetActive(false);
            Destroy(gameObject);
        }
    }

}

