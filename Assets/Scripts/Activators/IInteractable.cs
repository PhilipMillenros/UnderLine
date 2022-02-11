using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Press();
    public float Range { get; set; }
    
    public Vector3 Position { get; }
}
