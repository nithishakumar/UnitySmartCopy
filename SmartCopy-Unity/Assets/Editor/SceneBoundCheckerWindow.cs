using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SceneBoundCheckerWindow : EditorWindow
{
    private Object targetObject;

    [MenuItem("Tools/Scene Bound Checker")]
    public static void ShowWindow()
    {
        GetWindow<SceneBoundCheckerWindow>("Scene Bound Checker");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Drag Any Object Below", EditorStyles.boldLabel);

        targetObject = EditorGUILayout.ObjectField(
            "Target",
            targetObject,
            typeof(Object),
            true
        );

        if (targetObject == null)
            return;

        bool isSceneBound = EditorUnityObjectUtility.IsSceneBound(targetObject);
        bool isPersistent = EditorUtility.IsPersistent(targetObject);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Is Scene Bound:", isSceneBound.ToString());
        EditorGUILayout.LabelField("Is Persistent (Asset):", isPersistent.ToString());

        if (isSceneBound)
        {
            Scene scene = default;

            if (targetObject is GameObject go)
            {
                scene = go.scene;
            }
            else if (targetObject is Component comp)
            {
                scene = comp.gameObject.scene;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Scene Name:", scene.name);
            EditorGUILayout.LabelField("Scene Path:", scene.path);
        }

        if (GUILayout.Button("Log To Console"))
        {
            string log = $"{targetObject.name} ({targetObject.GetType().Name}) | " +
                         $"SceneBound: {isSceneBound} | Persistent: {isPersistent}";

            if (isSceneBound)
            {
                Scene scene = default;

                if (targetObject is GameObject go)
                    scene = go.scene;
                else if (targetObject is Component comp)
                    scene = comp.gameObject.scene;

                log += $" | Scene: {scene.name} ({scene.path})";
            }

            Debug.Log(log);
        }
    }
}