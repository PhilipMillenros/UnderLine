using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor.Line_Editor
{

    [CustomEditor(typeof(LineCreator))]
    public class LineEditor : UnityEditor.Editor
    {
        private LineCreator lineCreator;
        private void OnSceneGUI()
        {
            Input();
            Draw();
        }
        private void Input()
        {
            Event guiEvent = Event.current;
            Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
            {
                Undo.RecordObject(lineCreator, "Add segment");
                lineCreator.CreatePoint(new Vector3(mousePos.x, lineCreator.transform.position.y, mousePos.z));
            }
        }
        private void Draw()
        {
            for (int i = 0; i < lineCreator.Points.Count - 1; i++)
            {
                Handles.color = Color.white;
                Handles.DrawLine(lineCreator.Points[i].Position, lineCreator.Points[i + 1].Position);
            }
            for (int i = 0; i < lineCreator.Points.Count; i++)
            {
                Handles.color = Color.red;
                Vector3 newPos = Handles.FreeMoveHandle(lineCreator.Points[i].Position, Quaternion.identity, 0.1f, Vector2.zero, Handles.CylinderHandleCap);
                if (newPos != lineCreator.Points[i].Position)
                {
                    Undo.RecordObject(lineCreator, "Move segment");
                }
                lineCreator.Points[i].Position = new Vector3(newPos.x, lineCreator.transform.position.y, newPos.z);
            }
        }
        private void OnEnable()
        {
            lineCreator = (LineCreator) target;
            if (lineCreator.Points.Count < 1)
            {
                lineCreator.StartPoint();
            }
            
        }
    }
    
}