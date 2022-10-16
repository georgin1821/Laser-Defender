using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using System;

public class TaskListEditor : EditorWindow
{
    VisualElement container;
    TextField taskText;
    Button addTaskButton;
    ScrollView taskListScrollView;
    Button loadTasksButton;
    ObjectField savedTaskObjectField;
    TaskListSO taskListSO;


    [MenuItem("UIBuilder/Taks List")]
    public static void ShowWindow()
    {
        TaskListEditor editorWindow = GetWindow<TaskListEditor>();
        editorWindow.titleContent = new GUIContent("Task List");
    }

    public void CreateGUI()
    {
        container = rootVisualElement;
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Tasks/Editor/EditorWindow/TaskListEditorWindow.uxml");
        container.Add(visualTree.Instantiate());

        StyleSheet style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Tasks/Editor/EditorWindow/TaskListEditorUCSS.uss");
        container.styleSheets.Add(style);

        taskText = container.Q<TextField>("taskText");
        taskText.RegisterCallback<KeyDownEvent>(AddTask);
        addTaskButton = container.Q<Button>("addTaskButton");
        taskListScrollView = container.Q<ScrollView>("TaskList");
        loadTasksButton = container.Q<Button>("LoadTasksButton");
        savedTaskObjectField = container.Q<ObjectField>("SavedTasksObjectField");
        savedTaskObjectField.objectType = typeof(TaskListSO);
        addTaskButton.clicked += AddTask;
        loadTasksButton.clicked += LoadTasks;
    }

    void LoadTasks()
    {
        taskListSO = savedTaskObjectField.value as TaskListSO;
        if(taskListSO != null)
        {
            taskListScrollView.Clear();
            List<string> tasks = taskListSO.GetTasks();
            foreach (string task in tasks)
            {
                taskListScrollView.Add(CreateTask(task));
            }
        }
    }
    private void AddTask(KeyDownEvent evt)
    {
        if (Event.current.Equals(Event.KeyboardEvent("Return")))
        {
            AddTask();
        }
    }

    private void AddTask()
    {
        if (!string.IsNullOrEmpty(taskText.value))
        {
            taskListScrollView.Add(CreateTask(taskText.value));
            SaveTask(taskText.value);
            taskText.value = "";
            taskText.Focus();
        }
    }

    private Toggle CreateTask(string taskText)
    {
        Toggle taskItem = new Toggle();
        taskItem.text = taskText;
        return taskItem;
    }
    private void SaveTask(string task)
    {
        taskListSO.AddTask(task);
        EditorUtility.SetDirty(taskListSO);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
