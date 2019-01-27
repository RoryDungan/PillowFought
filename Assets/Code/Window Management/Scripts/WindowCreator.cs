using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// this class simply creates windows easily from anywhere
/// </summary>
public class WindowCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] windowStarter;
    [SerializeField]
    private RectTransform windowTrackingLayer;
    public static WindowCreator instance;

    public WindowSchematic roundFinishWindow;

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

        // chose which window we're using
        GameObject window = Instantiate(windowStarter[(int)windowScheme.type], this.transform);
        WindowContainer component = window.GetComponent<WindowContainer>();

        foreach (GameObject obj in windowScheme.uiObjects)
        {
            GameObject.Instantiate(obj, component.content);
        }

        component.ShowThisWindow(windowScheme.ScreenOrder);

        return window;
    }

    /// <summary>
    /// creates an object-trakcing UI element
    /// </summary>
    /// <returns>The trackable UIE lement.</returns>
    /// <param name="windowScheme">Window scheme.</param>
    /// <param name="objectToTrack">Object to track.</param>
    public GameObject CreateTrackableWindow(WindowSchematic windowScheme, GameObject objectToTrack)
    {
        GameObject newTrackableWindow = Instantiate(windowStarter[(int)windowScheme.type], windowTrackingLayer);
        TrackableUIElement component = newTrackableWindow.GetComponent<TrackableUIElement>();
        foreach (GameObject obj in windowScheme.uiObjects)
        {
            GameObject.Instantiate(obj, component.contents);
        }

        newTrackableWindow.GetComponent<TrackableUIElement>().StartTracking(objectToTrack);
        return newTrackableWindow;
    }

    public async void OnRoundFinish(int winningPlayer)
    {
        GameObject go = CreateWindow(roundFinishWindow);
        go.GetComponentInChildren<FinishWindow>().Populate(winningPlayer);

        await Task.Delay(2000);

        go.GetComponent<WindowContainer>().CloseWindow();
    }
}

/// <summary>
/// The different types of WiNdow in the app
/// </summary>
public enum WindowTypes
{
    SLIDE_FROM_BOTTOM_OVERLAY = 0,
    FADE_ON_SCREEN_OVERLAY = 1,
    TRACKING_FILL = 2,
    TRACKING_IMAGE = 3
}
