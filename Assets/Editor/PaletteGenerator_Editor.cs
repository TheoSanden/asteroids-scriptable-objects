using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

/*[CustomEditor(typeof(PaletteGenerator))]
public class PaletteGenerator_Editor : Editor
{
    PaletteGenerator pg;
    VisualElement root;
    VisualElement Root
    {
        get { return root; }
        set { root = value; BindTree(); }
    }
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement myInspector = new VisualElement();
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Tools/PaletteGenerator/Ui/PaletteGenerator.uxml");
        visualTree.CloneTree(myInspector);
        Root = myInspector;
        return myInspector;
    }
    void BindTree()
    {
        var generateButton = root.Q<Button>("Generate");
        generateButton.clicked += pg.Generate;
    }
    private void OnEnable()
    {
        pg = (PaletteGenerator)target;
    }
}*/
