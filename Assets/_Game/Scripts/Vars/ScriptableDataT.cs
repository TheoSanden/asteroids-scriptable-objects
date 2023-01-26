using System;
using UnityEngine;
public class ScriptableData<T> : ScriptableObject
{
    public delegate void OnValueChange();
    public event OnValueChange onValueChange;
    [SerializeField] private T _value;
    public T Data
    {
        set { _value = value; onValueChange?.Invoke(); }
        get => _value;
    }
}
