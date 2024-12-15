using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace ModularEventArchitecture.Editor
{
    public class SamplesEditorWindow : EditorWindow
    {
        private Vector2 scrollPosition;
        private Dictionary<string, bool> foldoutStates = new Dictionary<string, bool>();

        [MenuItem("Window/Modular Event Architecture/Samples Browser")]
        public static void ShowWindow()
        {
            var window = GetWindow<SamplesEditorWindow>();
            window.titleContent = new GUIContent("MEA Samples");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Modular Event Architecture Samples", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);
            
            string samplesPath = Path.Combine(Application.dataPath, "Scripts/Modular-event-architecture/Samples~");
            if (!Directory.Exists(samplesPath))
            {
                EditorGUILayout.HelpBox("Samples folder not found at path: " + samplesPath, MessageType.Info);
                return;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            var directories = Directory.GetDirectories(samplesPath);
            foreach (var dir in directories)
            {
                DrawSampleModule(dir);
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawSampleModule(string dir)
        {
            string dirName = Path.GetFileName(dir);
            if (!foldoutStates.ContainsKey(dirName))
            {
                foldoutStates[dirName] = false;
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.BeginHorizontal();
            foldoutStates[dirName] = EditorGUILayout.Foldout(foldoutStates[dirName], dirName, true);
            
            if (GUILayout.Button("Open in Explorer", GUILayout.Width(120)))
            {
                EditorUtility.RevealInFinder(dir);
            }
            EditorGUILayout.EndHorizontal();

            if (foldoutStates[dirName])
            {
                EditorGUI.indentLevel++;

                string packageJsonPath = Path.Combine(dir, "package.json");
                if (File.Exists(packageJsonPath))
                {
                    string jsonContent = File.ReadAllText(packageJsonPath);
                    var packageData = JsonUtility.FromJson<PackageInfo>(jsonContent);
                    
                    EditorGUILayout.LabelField("Version:", packageData.version);
                    EditorGUILayout.LabelField("Description:", packageData.description);
                    
                    if (packageData.dependencies != null)
                    {
                        EditorGUILayout.Space(5);
                        EditorGUILayout.LabelField("Dependencies:", EditorStyles.boldLabel);
                        foreach (var dep in packageData.dependencies.GetType().GetFields())
                        {
                            EditorGUILayout.LabelField($"- {dep.Name}: {dep.GetValue(packageData.dependencies)}");
                        }
                    }
                }

                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField("Structure:", EditorStyles.boldLabel);
                DrawDirectoryStructure(dir, 0);

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }

        private void DrawDirectoryStructure(string path, int indent)
        {
            EditorGUI.indentLevel += indent;

            // Показываем файлы
            foreach (var file in Directory.GetFiles(path))
            {
                string fileName = Path.GetFileName(file);
                if (!fileName.StartsWith(".")) // Пропускаем скрытые файлы
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("📄 " + fileName);
                    if (GUILayout.Button("View", GUILayout.Width(60)))
                    {
                        AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<Object>(GetRelativePath(file)));
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            // Рекурсивно показываем папки
            foreach (var dir in Directory.GetDirectories(path))
            {
                string dirName = Path.GetFileName(dir);
                if (!dirName.StartsWith(".")) // Пропускаем скрытые папки
                {
                    EditorGUILayout.LabelField("📁 " + dirName);
                    DrawDirectoryStructure(dir, 1);
                }
            }

            EditorGUI.indentLevel -= indent;
        }

        private string GetRelativePath(string fullPath)
        {
            return fullPath.Substring(Application.dataPath.Length - "Assets".Length)
                          .Replace('\\', '/');
        }

        private class PackageInfo
        {
            public string version;
            public string description;
            public Dependencies dependencies;
        }

        private class Dependencies
        {
            public string com_gamedevlab_modular_event_architecture;
        }
    }
}
