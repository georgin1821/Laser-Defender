using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using System;

public class EditorTest : EditorWindow
{
    VisualElement container;
    TextField taskText;
    Button addTask;
    ScrollView taskListScrollView;
    Toggle toggle;
    [MenuItem("ToolKit/test")]
    public static void ShowWindow()
    {
        EditorTest editorWindow = GetWindow<EditorTest>();
        editorWindow.minSize = new Vector2(200, 200);
        editorWindow.titleContent = new GUIContent("Task List");
    }

    public void CreateGUI()
    {
        container = rootVisualElement;
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/EditorWindow/TaskListWindow.uxml");
        container.Add(visualTree.Instantiate());

        StyleSheet style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/EditorWindow/style.uss");
        container.styleSheets.Add(style);


        toggle = container.Q<Toggle>("toggle");
        GameManager.Instance.isSpeedLevel = toggle.value;
    }

    private void AddTask()
    {
        
        
    }
}
