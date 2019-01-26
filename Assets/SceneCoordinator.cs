using System.Collections;
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
        //WindowCreator.instance.CreateWindow(startWindow);

        GameObject newProp1 = Instantiate(cubePrefab);
        GameObject newProp2 = Instantiate(cubePrefab);
        GameObject newProp3 = Instantiate(cubePrefab);

        WindowCreator.instance.CreateTrackableWindow(trackingWindow, newProp1);
        WindowCreator.instance.CreateTrackableWindow(trackingWindow, newProp2);
        WindowCreator.instance.CreateTrackableWindow(trackingWindow, newProp3);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
