using Oxygen_Line;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(OxygenLineCreator))]
    public class OxygenLineEditor : UnityEditor.Editor {

        private OxygenLineCreator creator;
        private SerializedProperty oxygenLinePath;
        private SerializedProperty oxygenLinePathPoints;
        private SerializedProperty oxygenLinePathOrigin;
        void OnSceneGUI()
        {
            Input();
            Draw();
            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        void Input()
        {
            Event guiEvent = Event.current;
            Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
            {
                Undo.RecordObject(creator, "Add segment");
                oxygenLinePathPoints.arraySize++;
                oxygenLinePathPoints.InsertArrayElementAtIndex(oxygenLinePathPoints.arraySize);
                oxygenLinePathPoints.GetArrayElementAtIndex(oxygenLinePathPoints.arraySize - 1).vector3Value =
                    new Vector3(mousePos.x, creator.transform.position.y, mousePos.z);
            }
        }

        void Draw()
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < oxygenLinePathPoints.arraySize - 1; i++)
            {
                Handles.DrawLine(oxygenLinePathPoints.GetArrayElementAtIndex(i).vector3Value, oxygenLinePathPoints.GetArrayElementAtIndex(i + 1).vector3Value);
            }
            Handles.color = Color.red;
            for (int i = 0; i < oxygenLinePathPoints.arraySize; i++)
            {
                Vector3 newPos = Handles.FreeMoveHandle(oxygenLinePathPoints.GetArrayElementAtIndex(i).vector3Value, Quaternion.identity, .1f, Vector2.zero, Handles.CylinderHandleCap);
                if (oxygenLinePathPoints.GetArrayElementAtIndex(i).vector3Value != newPos)
                {
                    Undo.RecordObject(creator, "Move point");
                    oxygenLinePathPoints.GetArrayElementAtIndex(i).vector3Value = new Vector3(newPos.x, creator.transform.position.y, newPos.z);
                }

                if (oxygenLinePathOrigin.vector3Value != creator.transform.position)
                {
                    oxygenLinePathPoints.GetArrayElementAtIndex(i).vector3Value += creator.transform.position - oxygenLinePathOrigin.vector3Value;
                }
            }
            oxygenLinePathOrigin.vector3Value = creator.transform.position;
        
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Create new Path"))
            {
                Undo.RecordObject(creator, "Create new Path");
                creator.CreateNewPath();
            }
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }

        void OnEnable()
        {
            creator = (OxygenLineCreator) target;
            oxygenLinePath = serializedObject.FindProperty(nameof(OxygenLineCreator.OxygenLinePath));
            oxygenLinePathPoints = oxygenLinePath.FindPropertyRelative(nameof(OxygenLinePath.Points));
            oxygenLinePathOrigin = oxygenLinePath.FindPropertyRelative(nameof(OxygenLinePath.Origin));
        }
    }
}