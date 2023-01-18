using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;


public class AstroidGenerator_Editor : EditorWindow
{
    [SerializeField]
    VisualTreeAsset UxmlFile;
    VisualElement root;

    [MenuItem("Tools/Asteroid Generator")]
    public static void ShowMyEditor()
    {
        EditorWindow window = GetWindow<AstroidGenerator_Editor>();
        window.titleContent = new GUIContent("Asteroid Generator");
    }
    public void CreateGUI()
    {
        UxmlFile.CloneTree(rootVisualElement);
    }
    void Bind()
    {
        rootVisualElement.Q<Button>();
    }

}
