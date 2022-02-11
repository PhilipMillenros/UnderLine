using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Point
{
    public Vector3 Position;
    public Quaternion Rotation;

    public Point(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}
