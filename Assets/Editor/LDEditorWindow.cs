using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using System;

public class LDEditorWindow : EditorWindow
{
    VisualElement container;
    Button testButton;
    Toggle toggle;
    ObjectField objectField;

    public bool isSpeed;

    [MenuItem("UIBuilder/LD test eitor")]
    public static void ShowWindow(GameManager gameManager)
    {
        LDEditorWindow editorWindow = GetWindow<LDEditorWindow>();
       // editorWindow.minSize = new Vector2(200, 200);
       // editorWindow.titleContent = new GUIContent("LD Editor");
    }
    private void CreateGUI()
    {
        container = rootVisualElement;
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Tasks/Editor/LDEditorWindow.uxml");
        container.Add(visualTree.Instantiate());

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Tasks/Editor/LDEditorCSS.uss");
        container.styleSheets.Add(styleSheet);

        testButton = container.Q<Button>("testButton");
        testButton.clicked += ButtonClicked;
        objectField = container.Q<ObjectField>("object");
        objectField.objectType = typeof(GamePlayController);
        toggle = container.Q<Toggle>("Toggle");
    }

    private void ButtonClicked()
    {
       // toggle.(GameManager.Instance.isSpeedLevel);
        GameManager.Instance.isSpeedLevel = true;
    }

}
