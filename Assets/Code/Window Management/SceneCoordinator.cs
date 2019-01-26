﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCoordinator : MonoBehaviour
{
    public WindowSchematic startWindow;
    public WindowSchematic trackingWindow;

    [SerializeField]
    private GameObject cubePrefab;

    // Start is called before the first frame update
    void Start()
    {
        WindowCreator.instance.CreateWindow(startWindow);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}