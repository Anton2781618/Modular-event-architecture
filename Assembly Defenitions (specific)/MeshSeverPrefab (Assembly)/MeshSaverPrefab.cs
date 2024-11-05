using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class MeshSaverPrefab : MonoBehaviour
{
    public MeshFilter targetMeshFilter;

    private const string ResourceFolderName = "MyResources";
    private const string PrefabFolderName = "Prefabs";
    private const string MeshFolderName = "Meshes";

    public void Save()
    {
        if (targetMeshFilter == null)
        {
            Debug.LogError("MeshFilter not assigned!");
            return;
        }

        string resourcePath = $"Assets/{ResourceFolderName}/";
        string prefabPath = $"{resourcePath}{PrefabFolderName}/";
        string meshPath = $"{resourcePath}{MeshFolderName}/";

        EnsureDirectoryExists(resourcePath);
        EnsureDirectoryExists(prefabPath);
        EnsureDirectoryExists(meshPath);

        // Generate unique names
        string uniqueIdentifier = DateTime.Now.ToString("yyyyMMddHHmmss");
        string uniqueMeshName = $"{targetMeshFilter.gameObject.name}_Mesh_{uniqueIdentifier}.asset";
        string uniquePrefabName = $"{targetMeshFilter.gameObject.name}_Prefab_{uniqueIdentifier}.prefab";

        // Save Mesh
        Mesh newMesh = UnityEngine.Object.Instantiate(targetMeshFilter.sharedMesh);
        string fullMeshPath = $"{meshPath}{uniqueMeshName}";
        AssetDatabase.CreateAsset(newMesh, fullMeshPath);
        AssetDatabase.SaveAssets();

        // Create new object for prefab
        GameObject newObject = new GameObject(targetMeshFilter.gameObject.name);
        MeshFilter newMeshFilter = newObject.AddComponent<MeshFilter>();
        newMeshFilter.sharedMesh = newMesh;

        // Add MeshRenderer and copy materials
        MeshRenderer newRenderer = newObject.AddComponent<MeshRenderer>();
        MeshRenderer originalRenderer = targetMeshFilter.GetComponent<MeshRenderer>();
        if (originalRenderer != null)
        {
            newRenderer.sharedMaterials = originalRenderer.sharedMaterials;
        }

        // Save Prefab
        string fullPrefabPath = $"{prefabPath}{uniquePrefabName}";
        bool success = false;
        PrefabUtility.SaveAsPrefabAsset(newObject, fullPrefabPath, out success);

        if (success)
        {
            Debug.Log($"Prefab saved successfully at {fullPrefabPath}");
            Debug.Log($"Mesh saved successfully at {fullMeshPath}");
        }
        else
        {
            Debug.LogError("Failed to save prefab!");
        }

        // Destroy the temporary object
        DestroyImmediate(newObject);

        AssetDatabase.Refresh();
    }

    private void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MeshSaverPrefab))]
public class MeshSaverPrefabEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MeshSaverPrefab meshSaver = (MeshSaverPrefab)target;
        if (GUILayout.Button("Save Prefab and Mesh"))
        {
            meshSaver.Save();
        }
    }
}
#endif