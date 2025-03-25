using UnityEngine;
using UnityEditor;

public class EditorTools : EditorWindow
{
    public GameObject[] objectsToChooseFrom;

    public int rotationMinX = 0;
    public int rotationMaxX = 360;
    public int rotationMinY = 0;
    public int rotationMaxY = 360;
    public int rotationMinZ = 0;
    public int rotationMaxZ = 360;

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

        float minX = rotationMinX;
        float maxX = rotationMaxX;
        float minY = rotationMinY;
        float maxY = rotationMaxY;
        float minZ = rotationMinZ;
        float maxZ = rotationMaxZ;

        GUILayout.Label("Rotation Range (X):");
        EditorGUILayout.MinMaxSlider(ref minX, ref maxX, 0, 360);
        rotationMinX = Mathf.FloorToInt(minX);
        rotationMaxX = Mathf.FloorToInt(maxX);
        GUILayout.Label($"Min: {rotationMinX}, Max: {rotationMaxX}");

        GUILayout.Label("Rotation Range (Y):");
        EditorGUILayout.MinMaxSlider(ref minY, ref maxY, 0, 360);
        rotationMinY = Mathf.FloorToInt(minY);
        rotationMaxY = Mathf.FloorToInt(maxY);
        GUILayout.Label($"Min: {rotationMinY}, Max: {rotationMaxY}");

        GUILayout.Label("Rotation Range (Z):");
        EditorGUILayout.MinMaxSlider(ref minZ, ref maxZ, 0, 360);
        rotationMinZ = Mathf.FloorToInt(minZ);
        rotationMaxZ = Mathf.FloorToInt(maxZ);
        GUILayout.Label($"Min: {rotationMinZ}, Max: {rotationMaxZ}");

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

        int randomRotationX = Random.Range(rotationMinX, rotationMaxX + 1);
        int randomRotationY = Random.Range(rotationMinY, rotationMaxY + 1);
        int randomRotationZ = Random.Range(rotationMinZ, rotationMaxZ + 1);
        Quaternion rotation = Quaternion.Euler(randomRotationX, randomRotationY, randomRotationZ);

        DestroyImmediate(selectedObject);

        GameObject newObject = PrefabUtility.InstantiatePrefab(randomObject) as GameObject;
        newObject.transform.position = position;
        newObject.transform.rotation = rotation;

        EditorUtility.SetDirty(newObject);

        Selection.activeGameObject = newObject;

        Debug.Log("Replaced " + selectedObjectName + " with " + randomObject.name);
    }
}