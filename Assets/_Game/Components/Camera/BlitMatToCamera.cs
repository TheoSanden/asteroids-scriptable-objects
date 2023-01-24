using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlitMatToCamera : MonoBehaviour
{
    [SerializeField] PaletteStorage storage;
    public Material blitMat;

    void UpdateBlitMaterial()
    {
        blitMat.SetTexture("_ReplacementLut", storage.GetCurrentPallette().Palette);
    }
    private void Start()
    {
        storage.onCurrentPaletteChange += UpdateBlitMaterial;
    }
    void OnRenderImage(RenderTexture src, RenderTexture tar)
    {
        Graphics.Blit(src, tar, blitMat);
    }
}
