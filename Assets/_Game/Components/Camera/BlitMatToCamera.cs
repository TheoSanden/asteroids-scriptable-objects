using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlitMatToCamera : MonoBehaviour
{
    public Material blitMat; 
    void OnRenderImage(RenderTexture src,RenderTexture tar)
    {
       Graphics.Blit(src, tar, blitMat);
    }
}
