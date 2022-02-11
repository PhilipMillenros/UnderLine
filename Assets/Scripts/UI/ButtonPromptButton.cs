using System;
using UnityEngine;
using TMPro;

public class ButtonPromptButton : MonoBehaviour
{
    private TMP_Text text;
    private Transform objToFollow;
    private Camera cam;
    private bool isSetup = false;

    private int assignedID = -1;

    public int AssignedID
    {
        get { return assignedID; }
        set { assignedID = value; }
    }
    
    public void Setup(String inText, Transform inTrans)
    {
        text = GetComponentInChildren<TMP_Text>();
        text.text = inText;
        objToFollow = inTrans;
        cam = Camera.main;
        isSetup = true;
    }

    private void Update()
    {
        if(!isSetup) return;

        Vector3 onScreenPos = cam.WorldToScreenPoint(objToFollow.position);
        
        transform.position = onScreenPos;
    }
    
    
}
