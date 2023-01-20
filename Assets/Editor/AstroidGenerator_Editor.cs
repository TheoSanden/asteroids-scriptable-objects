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
    VisualElement outlinePreview;
    VisualElement texturePreview;
    [SerializeField]
    AstroidGenerator astroidGenerator;
    [SerializeField]
    Texture2D workload;
    [MenuItem("Tools/Asteroid Generator")]
    public static void ShowMyEditor()
    {
        EditorWindow window = GetWindow<AstroidGenerator_Editor>();
        window.titleContent = new GUIContent("Asteroid Generator");
    }
    public void CreateGUI()
    {
        UxmlFile.CloneTree(rootVisualElement);
        outlinePreview = rootVisualElement.Q<VisualElement>("OutlinePreview");
        texturePreview = rootVisualElement.Q<VisualElement>("TexturePreview");
        Bind();
    }
    void Save() 
    {
       
    }
    void Bind()
    {
        astroidGenerator = new AstroidGenerator();
        rootVisualElement.Bind(new SerializedObject(astroidGenerator));
        rootVisualElement.Bind(new SerializedObject(astroidGenerator.TexGenerator));
        var generateOutline = rootVisualElement.Q<Button>("GenerateOutline");
        var applyTexture = rootVisualElement.Q<Button>("ApplyTexture");
        generateOutline.clicked += GenerateOutline;
        applyTexture.clicked += ApplyTexture;
    }
    void GenerateOutline()
    {
        astroidGenerator.GenerateOutline();
        outlinePreview.style.backgroundImage = astroidGenerator.Workload;
    }
    void ApplyTexture()
    {
        astroidGenerator.ApplyTexture();
        outlinePreview.style.backgroundImage = astroidGenerator.Workload;
        texturePreview.style.backgroundImage = astroidGenerator.TexGenerator.Workload;
    }
}
