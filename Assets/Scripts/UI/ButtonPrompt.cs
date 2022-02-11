using System;
using TMPro;
using UnityEngine;

public class ButtonPrompt : MonoBehaviour
{
    [SerializeField] private GameObject buttonObject;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject buttonPrefab;

    private Camera mainCamera;

    private PlayerTriggerButtonPrompt playerBtnPrmpt;
    [SerializeField] private GameObject[] promptsList;

    
    private void Start()
    {
        buttonObject.SetActive(false);
        text = buttonObject.GetComponentInChildren<TMP_Text>();
        if(ReferenceEquals(buttonObject, null)) Debug.LogWarning("No button object linked");
        if(ReferenceEquals(text, null)) Debug.Log("rip life :/");

        promptsList = new GameObject[10];

        playerBtnPrmpt = FindObjectOfType<PlayerTriggerButtonPrompt>();
    }

    public void ActivateButtonPrompt(String button)
    {
        // add animation or something
        text.text = button;
        buttonObject.SetActive(true);
    }

    public void DeActivateButtonPrompt()
    {
        // add animation or something
        buttonObject.SetActive(false);
    }

    public void InstatiateNewPrompt(String button, Transform obj)
    {
        var objTrigg = obj.GetComponent<ObjectButtonPromptTriggerer>();
        int id = objTrigg.AssignedID;
        
        if(id >= 0 && id < promptsList.Length) 
            return;
        
        if(objTrigg.NeedsEel)
            {if(!playerBtnPrmpt.HasEel)
                return;}
        
        GameObject newPrompt = Instantiate(buttonPrefab);
        newPrompt.GetComponent<ButtonPromptButton>().Setup(button, obj);
        newPrompt.transform.SetParent(transform);
        for(int i = 0; i < promptsList.Length; i++)
        {
            if(promptsList[i] == null)
            {
                promptsList[i] = newPrompt;
                objTrigg.AssignedID = i;
                newPrompt.GetComponent<ButtonPromptButton>().AssignedID = i;
                break;
            }
        }
    }

    public void HidePrompt(bool hide, int id)
    {
        if(id < 0 || id >= promptsList.Length) 
            return;
        hide = !hide;
        promptsList[id].SetActive(hide);
    }

    public void DeletePrompt(Transform obj)
    {
        var objTrigg = obj.GetComponent<ObjectButtonPromptTriggerer>();
        int id = objTrigg.AssignedID;
        
        if(id < 0 || id >= promptsList.Length) 
            return;
        
        Destroy(promptsList[id]);
        promptsList[id] = null;
        objTrigg.AssignedID = -1;
    }
}
