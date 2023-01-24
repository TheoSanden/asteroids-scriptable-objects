using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PalletteInfo
{
    PalletteInfo(string name, Texture2D palette)
    {
        this.name = name;
        this.palette = palette;
    }
    [SerializeField] private string name;
    public string Name
    {
        get => name;
    }
    [SerializeField] private Texture2D palette;
    public Texture2D Palette
    {
        get => palette;
    }
}
[CreateAssetMenu(fileName = "new PaletteStorage", menuName = "ScriptableObjects/PaletteStorage")]
public class PaletteStorage : ScriptableObject
{
    public delegate void OnCurrentPaletteChange();
    public event OnCurrentPaletteChange onCurrentPaletteChange;
    [SerializeField] List<PalletteInfo> info = new List<PalletteInfo>();
    public int Size
    {
        get => info.Count;
    }
    [SerializeField] int current;
    public int Current
    {
        set { current = value; onCurrentPaletteChange?.Invoke(); }
        get => current;
    }
    public PalletteInfo GetCurrentPallette()
    {
        return info[current];
    }
}
