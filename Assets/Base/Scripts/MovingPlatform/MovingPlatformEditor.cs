using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
    private MovingPlatform platform;

    private void OnEnable()
    {
        platform = (MovingPlatform)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Waypoint Editor", EditorStyles.boldLabel);

        // Кнопка для добавления новой точки
        if (GUILayout.Button("Add Waypoint"))
        {
            Undo.RecordObject(platform, "Add Waypoint");
            ArrayUtility.Add(ref platform.waypoints, platform.transform.position);
        }

        // Кнопка для удаления последней точки
        if (platform.waypoints.Length > 0 && GUILayout.Button("Remove Last Waypoint"))
        {
            Undo.RecordObject(platform, "Remove Last Waypoint");
            ArrayUtility.RemoveAt(ref platform.waypoints, platform.waypoints.Length - 1);
        }

        // Применение изменений
        if (GUI.changed)
        {
            EditorUtility.SetDirty(platform);
        }
    }

    private void OnSceneGUI()
    {
        // Редактирование точек в сцене
        for (int i = 0; i < platform.waypoints.Length; i++)
        {
            Vector3 currentWaypoint = platform.waypoints[i];

            // Отображение точек для редактирования
            EditorGUI.BeginChangeCheck();
            Vector3 newWaypoint = Handles.PositionHandle(currentWaypoint, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(platform, "Move Waypoint");
                platform.waypoints[i] = newWaypoint;
            }
        }
    }
}