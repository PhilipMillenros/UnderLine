using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Oxygen_Line;
using Oxygen_Path;
using UnityEngine;

public class OxygenLineCreator : MonoBehaviour
{
	
	[SerializeField, HideInInspector] public OxygenLinePath OxygenLinePath;

	public void CreateNewPath()
	{
		OxygenLinePath = new OxygenLinePath(transform.position);
		OxygenLinePath.Points.Add(transform.position);
	}
}