using UnityEngine;
using UnityEditor;

public class EditorTools : EditorWindow
{
    public GameObject[] objectsToChooseFrom;

    [MenuItem("Tools/Randomize Selected Object")]
    public static void ShowWindow()
    {
        GetWindow<EditorTools>("Randomize Object");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Objects to Randomize", EditorStyles.boldLabel);
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty objectsProperty = serializedObject.FindProperty("objectsToChooseFrom");

        EditorGUILayout.PropertyField(objectsProperty, true);

        if (GUILayout.Button("Randomize Selected Object"))
        {
            RandomizeSelectedObject();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void RandomizeSelectedObject()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("Please select a GameObject to randomize.");
            return;
        }

        GameObject selectedObject = Selection.activeGameObject;

        if (objectsToChooseFrom.Length == 0)
        {
            Debug.LogWarning("No objects to choose from.");
            return;
        }

        string selectedObjectName = selectedObject.name;

        GameObject randomObject = objectsToChooseFrom[Random.Range(0, objectsToChooseFrom.Length)];

        Vector3 position = selectedObject.transform.position;
        Quaternion rotation = selectedObject.transform.rotation;

        DestroyImmediate(selectedObject);

        GameObject newObject = PrefabUtility.InstantiatePrefab(randomObject) as GameObject;
        newObject.transform.position = position;
        newObject.transform.rotation = rotation;

        EditorUtility.SetDirty(newObject);

        Selection.activeGameObject = newObject;

        Debug.Log("Replaced " + selectedObjectName + " with " + randomObject.name);
    }
}