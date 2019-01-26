using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this ScriptableObject holds the properties behind a window, including where it is created
/// </summary>
[CreateAssetMenu(fileName = "WindowSchematic", menuName = "Window Scheme", order = 0)]
public class WindowSchematic : ScriptableObject
{
    public bool useBackStack = false;
    public GameObject[] uiObjects;
    public int ScreenOrder;
    public string windowName;
    public WindowTypes type;
}
