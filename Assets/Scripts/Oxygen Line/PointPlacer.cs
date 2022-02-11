using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Oxygen_Line;
using Oxygen_Path;
using Unity.Mathematics;
using UnityEngine;

public class PointPlacer : MonoBehaviour
{
    private OxygenLineCreator oxygenLineCreator;
    private OxygenLine oxygenLine;
    private void Awake()
    {
        oxygenLine = GetComponent<OxygenLine>();
        oxygenLineCreator = GetComponent<OxygenLineCreator>();
        SetEvenlySpacePoints();
    }

    private void SetEvenlySpacePoints()
    {
        if (oxygenLineCreator.OxygenLinePath.Points.Count < 2)
        {
            return;
        }
        Vector3 previousPosition = oxygenLineCreator.OxygenLinePath.Points[0];
        for (int i = 0; i < oxygenLineCreator.OxygenLinePath.Points.Count - 1; i++)
        {
            Vector3 point1 = oxygenLineCreator.OxygenLinePath.Points[i];
            Vector3 point2 = oxygenLineCreator.OxygenLinePath.Points[i + 1];
            
            for (float t = 0; t < 1; t += 0.01f)
            {
                Vector3 currentPosition = Vector3.Lerp(point1, point2, t);
                Vector3 offset = currentPosition - previousPosition;
                if (offset.sqrMagnitude > oxygenLine.DistancePerPoint * oxygenLine.DistancePerPoint)
                {
                    oxygenLine.Points.Add(new Point(currentPosition, CalculateRotation(point1, point2)));
                    previousPosition = currentPosition;
                }
            }
        }
        oxygenLine.Points[oxygenLine.Points.Count - 1].Rotation = oxygenLine.Points[oxygenLine.Points.Count - 2].Rotation;
    }

    private Quaternion CalculateRotation(Vector3 point1, Vector3 point2)
    {
        Quaternion rotation = Quaternion.LookRotation(point2 - point1, Vector3.up);
        return rotation;
    }
    
}
