using System;
using UnityEngine;

public class ObjectButtonPromptTriggerer : MonoBehaviour
{
    [SerializeField] private String button;

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightedMaterial;

    [SerializeField] private bool objHaveSkinnedMeshRenderers;

    [SerializeField] private SkinnedMeshRenderer[] skinnedMeshRenderers;
    [SerializeField] private MeshRenderer[] meshRenderers;

    [SerializeField][Tooltip("Ignore. 'tis for debugging purposes")] private int assignedID = -1;

    [SerializeField] private bool isEel;
    [SerializeField] private bool needsEel;
    
    public bool IsEel
    {
        get { return isEel; }
    }

    public bool NeedsEel
    {
        get { return needsEel; }
    }

    public int AssignedID
    {
        get { return assignedID; }
        set { assignedID = value; }
    }

    public String Button
    {
        get { return button; }
    }

    public void SwitchMaterial(bool inRange)
    {
        switch(inRange)
        {
            case true:
                switch(objHaveSkinnedMeshRenderers)
                {
                    case true:
                        for(int i = 0; i < skinnedMeshRenderers.Length; i++)
                            skinnedMeshRenderers[i].material = highlightedMaterial;
                        break;
                    case false:
                        for(int i = 0; i < meshRenderers.Length; i++)
                            meshRenderers[i].material = highlightedMaterial;
                        break;
                }
                break;
            case false:
                switch(objHaveSkinnedMeshRenderers)
                {
                    case true:
                        for(int i = 0; i < skinnedMeshRenderers.Length; i++)
                            skinnedMeshRenderers[i].material = defaultMaterial;
                        break;
                    case false:
                        for(int i = 0; i < meshRenderers.Length; i++)
                            meshRenderers[i].material = defaultMaterial;
                        break;
                }
                break;
        }
    }
}
