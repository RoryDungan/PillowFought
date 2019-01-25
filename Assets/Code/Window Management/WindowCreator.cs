using UnityEngine;

/// <summary>
/// this class simply creates windows easily from anywhere
/// </summary>
public class WindowCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject windowStarter;

    public static WindowCreator instance;

    public void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// this function is responsible for creating the various window it is passed
    /// </summary>
    /// <param name="windowScheme">Window scheme.</param>
    public GameObject CreateWindow(WindowSchematic windowScheme)
    {
        Debug.Log($"Creating the window {windowScheme.windowName}");
        GameObject window = Instantiate(windowStarter, this.transform);
        WindowContainer component = window.AddComponent<WindowContainer>();

        foreach (GameObject obj in windowScheme.uiObjects)
        {
            GameObject.Instantiate(obj, window.transform);
        }

        component.ShowThisWindow(windowScheme.ScreenOrder);

        return window;
    }
}
