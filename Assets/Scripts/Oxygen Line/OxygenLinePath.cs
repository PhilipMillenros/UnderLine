using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oxygen_Line
{
    [Serializable]
    public class OxygenLinePath
    {
        [SerializeField] public List<Vector3> Points = new List<Vector3>();
        [SerializeField] public Vector3 Origin;
        public OxygenLinePath(Vector3 position)
        {
            Origin = position;
        }
    }
}