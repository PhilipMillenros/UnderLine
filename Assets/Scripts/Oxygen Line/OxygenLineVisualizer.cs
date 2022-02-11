
using System;
using System.Collections.Generic;
using Oxygen_Line;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Oxygen_Path
{
    public class OxygenLineVisualizer : MonoBehaviour
    {
        [SerializeField] private OxygenLine oxygenLine;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Color StartColor, EndColor;
        [SerializeField] private Transform origin;
        private List<Point> points;
        private int previousPointCount;
        private float maxLength => oxygenLine.MaxLength;
        private float currentLength => oxygenLine.CurrentLength;

        private void Start()
        {
            points = oxygenLine.Points;
            DrawOxygenLine();
        }

        private void Update()
        {
            DrawOxygenLine();
        }

        private void DrawOxygenLine()
        {
            if (points.Count < 2)
            {
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, origin.position - lineRenderer.transform.position);
                if (oxygenLine.IsConnectedToPlayer)
                {
                    lineRenderer.SetPosition(1, Player.player.origin.transform.position - lineRenderer.transform.position);
                }
                else
                {
                    lineRenderer.SetPosition(1, oxygenLine.ConnectionPoint.nozzle.transform.position - lineRenderer.transform.position);
                }
                return;
            }
                
            lineRenderer.positionCount = points.Count;
            SetColor();
            lineRenderer.SetPosition(0, origin.position - lineRenderer.transform.position);
            for (int i = 1; i < lineRenderer.positionCount; i ++)
            {
                lineRenderer.SetPosition(i, points[i].Position - lineRenderer.transform.position);
            }
        }

        private void SetColor()
        {
            var block = new MaterialPropertyBlock();
            block.SetColor("Color_5e925b7b40b34f5498475f67824c9887",
                Color.Lerp(StartColor, EndColor, currentLength / maxLength));
            lineRenderer.SetPropertyBlock(block);

        }
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                DrawPoints();
            }
        }
        private void DrawPoints()
        {
            for (int i = 0; i < points.Count; i++)
            {
                Gizmos.DrawWireSphere(points[i].Position, .15f);
                Handles.color = Color.blue;
                Handles.ArrowHandleCap(0, points[i].Position, points[i].Rotation, 0.25f, EventType.Repaint);
            }

            if (points.Count > 0)
            {
                Handles.color = Color.green;
                Handles.Label(points[points.Count - 1].Position + Vector3.up * 4,
                    $"{currentLength} / {maxLength}");
            }
            
        }
        #endif
    }
}
