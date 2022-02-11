using System;
using Oxygen_Path;
using UnityEngine;

namespace Oxygen_Line
{
    public class OxygenLineConnector : MonoBehaviour
    {
        private OxygenLine closestOxygenLine;
        private OxygenLine equipedOxygenLine;
        [SerializeField] private OxygenLine startingOxygenLine;

        private void Start()
        {
            SetStartingOxygenLine();
            transform.position = startingOxygenLine.ConnectionPoint.transform.position;
            transform.rotation = startingOxygenLine.ConnectionPoint.transform.rotation;
        }
        private void SetStartingOxygenLine()
        {
            if (startingOxygenLine.Points.Count > 0)
            {
                Transform connectionPoint = startingOxygenLine.ConnectionPoint.transform;
                connectionPoint.position =
                    startingOxygenLine.Points[startingOxygenLine.Points.Count - 1].Position;
                connectionPoint.rotation =
                    startingOxygenLine.Points[startingOxygenLine.Points.Count - 1].Rotation;
            }

            startingOxygenLine.ConnectionPoint.Connect(transform);
            equipedOxygenLine = startingOxygenLine;
        }
        private void Update()
        {
            SetClosestOxygenLine();
        }

        private void SetClosestOxygenLine()
        {
            closestOxygenLine = FindClosestOxygenLine();
            
        }
        private OxygenLine FindClosestOxygenLine()
        {
            int count = OxygenLine.OxygenLines.Count;
            float closest = Mathf.Infinity;
            int index = 0;
            if (count < 2)
                return OxygenLine.OxygenLines[0];
            for (int i = 0; i < count; i++)
            {
                float squaredDistance = (OxygenLine.OxygenLines[i].ConnectionPoint.transform.position 
                                         - transform.position).sqrMagnitude;
                if (closest > squaredDistance && equipedOxygenLine != OxygenLine.OxygenLines[i])
                {
                    closest = squaredDistance;
                    index = i;
                }
            }
            return OxygenLine.OxygenLines[index];
        }

        private void OnConnect()
        {
            Connect();
        }

        private void Connect()
        {
            if (closestOxygenLine != equipedOxygenLine && closestOxygenLine.ConnectionPoint.TryConnecting(transform))
            {
//                FG.AudioManager.Instance.Play("o2 Dissconecting");
                equipedOxygenLine = closestOxygenLine;
                transform.rotation = equipedOxygenLine.ConnectionPoint.Point.Rotation;
            }
        }
    }
}
