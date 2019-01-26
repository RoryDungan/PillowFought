using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCoordinator : MonoBehaviour
{
    public WindowSchematic startWindow;
    public WindowSchematic finishWindow;
    public WindowSchematic trackingWindow;

    private GameObject startWindowInstance;
    private GameObject finishWindowInstance;

    [SerializeField]
    private GameObject cubePrefab;

    // Start is called before the first frame update
    void Start()
    {
        startWindowInstance = WindowCreator.instance.CreateWindow(startWindow);

    }
}
