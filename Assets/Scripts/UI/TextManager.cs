using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour
{
    public GameObject UIObject;
    public Animator animator;


    private void Start()
    {
        UIObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        UIObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        //animator.SetTrigger("TextFadeOut");
        //UIObject.SetActive(false);
    }
}
