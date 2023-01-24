using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PalleteMenu_UI : MonoBehaviour
{
    [SerializeField] PaletteStorage storage;
    [SerializeField] UIDocument uiDocument;

    Label label;
    Button startButton;
    private void Step(int step)
    {
        step = (step < 0) ? -1 : (step > 0) ? 1 : 0;
        storage.Current = (storage.Current + step == storage.Size) ? 0 : (storage.Current + step == -1) ? storage.Size - 1 : storage.Current + step;
    }
    private void UpdateCurrentPalette()
    {
        label.text = storage.GetCurrentPallette().Name;
        Color uiColor = storage.GetCurrentPallette().Palette.GetPixel(4, 0);
        label.style.color = uiColor;
        startButton.style.color = uiColor;
    }
    private void OnEnable()
    {
        Bind();
        UpdateCurrentPalette();
        storage.onCurrentPaletteChange += UpdateCurrentPalette;
    }
    private void OnDisable()
    {
        storage.onCurrentPaletteChange -= UpdateCurrentPalette;
    }
    private void Bind()
    {
        var leftButton = uiDocument.rootVisualElement.Q<Button>("Left");
        var rightButton = uiDocument.rootVisualElement.Q<Button>("Right");
        label = uiDocument.rootVisualElement.Q<Label>("PaletteName");
        leftButton.clicked += OnClickedLeftButton;
        rightButton.clicked += OnClickedRightButton;

        //Move this
        startButton = uiDocument.rootVisualElement.Q<Button>("Start");
        startButton.clicked += MoveThisIntoMainMenuScriptInstead;
    }
    private void UnBind()
    {
        var leftButton = uiDocument.rootVisualElement.Q<Button>("Left");
        var rightButton = uiDocument.rootVisualElement.Q<Button>("Right");
        leftButton.clicked -= OnClickedLeftButton;
        rightButton.clicked -= OnClickedRightButton;

        //Move this
        startButton = uiDocument.rootVisualElement.Q<Button>("Start");
        startButton.clicked -= MoveThisIntoMainMenuScriptInstead;
    }
    void OnClickedLeftButton()
    {
        Step(-1);
    }
    void OnClickedRightButton()
    {
        Step(1);
    }
    void MoveThisIntoMainMenuScriptInstead()
    {
        SceneManager.LoadScene(1);
    }
}
